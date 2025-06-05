using CranchyLib.Networking;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Media;
using System.Threading.Tasks;
using System.Windows.Forms;

using static SteamCosmetics.ApplicationPath;
using static SteamCosmetics.SessionData;






namespace SteamCosmetics
{
    public partial class Form_Main : Form
    {
        private void CloseSelf()
        {
            Process.GetCurrentProcess().Kill();
        }
        private void CriticalError(string text)
        {
            MessageBox.Show(text, "Steam Cosmetics Critical Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            CloseSelf();
        }




        private bool IsSteamRunning()
        {
            Process[] processes = Process.GetProcessesByName("steam");
            return processes.Length > 0;
        }
        private bool IsGameRunning()
        {
            Process[] processes = Process.GetProcessesByName("DeadByDaylight-Win64-Shipping");
            return processes.Length > 0;
        }
        private async Task GameProcessLookupHandler()
        {
            while (true)
            {
                if (IsGameRunning() == true)
                {
                    CriticalError("Dead By Daylight process has been detected!");
                    Close();
                }

                await Task.Delay(1000);
            }
        }




        private void ValidateWorkingEnvironment()
        {
            if (IsSteamRunning() == false)
            {
                CriticalError("Steam Client is not running!");
            }

            if (IsGameRunning())
            {
                CriticalError("Dead by Daylight shouldn't be running!");
            }

            Task.Run(() => GameProcessLookupHandler());
        }




        private void InitializeCharactersMap()
        {
            if (File.Exists(charactersMapPath) == false)
            {
                CriticalError($"Failed to find \"{charactersMapPath}\"!");
            }

            string charactersMap = File.ReadAllText(charactersMapPath);
            if (charactersMap.IsJson() == false)
            {
                CriticalError($"Failed to read \"{charactersMapPath}\"!");
            }

            SetCharactersMap(JObject.Parse(charactersMap));
        }


        private void InitializeBhvrSession()
        {
            if (File.Exists("bhvrSession.txt") == false)
                CriticalError("\"bhvrSession.txt\" file is missing!");

            string bhvrSessionFileContents = File.ReadAllText("bhvrSession.txt")
                .Replace("Cookie: ", string.Empty)
                .Replace("bhvrSession=", string.Empty);

            if (string.IsNullOrEmpty(bhvrSessionFileContents))
                CriticalError("\"bhvrSession.txt\" file is empty!");


            List<string> headers = new List<string>
            {
                $"Cookie: bhvrSession={bhvrSessionFileContents}"
            };
            var gameConfigResponse = Networking.Get("https://steam.live.bhvrdbd.com/api/v1/config", headers);
            if (gameConfigResponse.statusCode != Networking.E_StatusCode.OK)
                CriticalError("bhvrSession is invalid or game server are down!");


            SetBhvrSession(bhvrSessionFileContents);
        }


        private void InitializeFullProfile()
        {
            FullProfile.S_Profile fullProfile = FullProfile.Get(GetBhvrSession());
            if (fullProfile.version == -1)
            {
                CriticalError("Failed to retrieve fullProfile!");
            }


            if (fullProfile.content.ContainsKey("characterCustomizationPresets") == false)
            {
                CriticalError("Failed to retrieve characterCustomizationPresets!");
            }


            SetFullProfile(fullProfile);
        }




        private S_CosmeticsPreset GetEquipedCosmetics(string characterId)
        {
            JObject fullProfileContent = GetFullProfileContent();
            int loadoutSlot = GetSelectedLoadoutSlot();

            foreach (var customizationPreset in fullProfileContent["characterCustomizationPresets"])
            {
                if ((string)customizationPreset["characterId"] == characterId)
                {
                    return new S_CosmeticsPreset((string)customizationPreset["presets"][loadoutSlot]["head"], (string)customizationPreset["presets"][loadoutSlot]["torsoOrBody"], (string)customizationPreset["presets"][loadoutSlot]["legsOrWeapon"]);
                }
            }

            return new S_CosmeticsPreset(null, null, null);
        }
        private void SetEquipedCosmetics(string characterId, S_CosmeticsPreset newPreset, bool commitChanges = false)
        {
            JObject fullProfileContent = GetFullProfileContent();
            int loadoutSlot = GetSelectedLoadoutSlot();


            JToken charactersCustomizationPresets = fullProfileContent["characterCustomizationPresets"];
            bool changesHasBeenMade = false;
            foreach (var customizationPreset in charactersCustomizationPresets)
            {
                if ((string)customizationPreset["characterId"] == characterId)
                {
                    JToken customizations = customizationPreset["presets"][loadoutSlot];
                    customizations["head"] = newPreset.head;
                    customizations["torsoOrBody"] = newPreset.torsoOrBody;
                    customizations["legsOrWeapon"] = newPreset.legsOrWeapon;

                    changesHasBeenMade = true;
                    break;
                }
            }

            if (changesHasBeenMade)
            {
                SetFullProfileContent(fullProfileContent);
            }


            if (commitChanges == true)
            {
                if (FullProfile.Set(GetBhvrSession(), GetFullProfile()) == false)
                {
                    CriticalError("Failed to upload a new fullProfile!");
                }
                else
                {
                    SoundPlayer soundPlayer = new SoundPlayer();
                    soundPlayer.Stream = Properties.Resources.SFX_Activate;
                    soundPlayer.Play();

                    SetFullProfileVersion(GetFullProfileVersion() + 1);
                }
            }
        }




        private void UpdateCharacterPreview()
        {
            string characterId = GetSelectedCharacterId();
            string imagePath = Path.Combine(charactersPortraitsDirectoryPath, (string)GetCharactersMap()[characterId]["charPortrait"]);

            pictureBox_CharacterPortrait.Image = File.Exists(imagePath) ? Image.FromFile(imagePath) : Properties.Resources.Char_Missing;

            S_CosmeticsPreset equipedCosmetics = GetEquipedCosmetics(characterId);
            textBox_Head.Text = equipedCosmetics.head;
            textBox_TorsoBody.Text = equipedCosmetics.torsoOrBody;
            textBox_LegsWeapon.Text = equipedCosmetics.legsOrWeapon;
        }


        private void UpdateEquipedCosmetics()
        {
            SetEquipedCosmetics(GetSelectedCharacterId(), new S_CosmeticsPreset(textBox_Head.Text, textBox_TorsoBody.Text, textBox_LegsWeapon.Text), false);
        }
        private void CommitEquipedCosmetics()
        {
            SetEquipedCosmetics(GetSelectedCharacterId(), new S_CosmeticsPreset(textBox_Head.Text, textBox_TorsoBody.Text, textBox_LegsWeapon.Text), true);
        }




        public Form_Main()
        {
            InitializeComponent();
        }
        private void Form_Main_Load(object sender, EventArgs e)
        {
            ValidateWorkingEnvironment();


            InitializeCharactersMap();
            InitializeBhvrSession();
            InitializeFullProfile();


            JObject charactersMap = GetCharactersMap();
            foreach (var character in charactersMap)
            {
                string friendlyName = (string)character.Value["friendlyName"];

                if (friendlyName != null)
                {
                    int newItemIndex = comboBox_CharacterSelect.Items.Add(friendlyName);
                    characterSelectComboBoxMappings.Add(newItemIndex, character.Key);
                }
            }


            comboBox_CharacterSelect.SelectedIndex = 0;
        }




        private void button_WindowClose_MouseClick(object sender, EventArgs e) => CloseSelf();
        private void button_WindowMinimize_MouseClick(object sender, EventArgs e) => this.WindowState = FormWindowState.Minimized;
        private async void panel_WindowHeader_MouseDown(object sender, MouseEventArgs e)
        {
            panel_WindowHeader.Capture = false;

            await Task.Run(() =>
            {
                this.Invoke(new Action(() =>
                {
                    Message mouse = Message.Create(Handle, 0xa1, new IntPtr(2), IntPtr.Zero); // 0xA1 - WM_NCLBUTTONDOWN (Posted when the user presses the left mouse button while the cursor is within the nonclient area of a window) | new IntPtr(2) - HTCAPTION (We're making system aware that we have pressed LMB in window title area) | IntPtr.Zero - lParam (Unused in our scenario)
                    WndProc(ref mouse);
                }));
            });
        }




        private void comboBox_Characters_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetSelectedCharacterId(characterSelectComboBoxMappings[comboBox_CharacterSelect.SelectedIndex]);
            UpdateCharacterPreview();
        }
        private void trackBar_LoadoutSlot_Scroll(object sender, EventArgs e)
        {
            label_LoadoutSlot.Text = $"Loadout Slot #{trackBar_LoadoutSlot.Value}";
            SetSelectedLoadoutSlot(trackBar_LoadoutSlot.Value - 1);

            UpdateCharacterPreview();
        }




        private void button_Update_MouseClick(object sender, MouseEventArgs e)
        {
            CommitEquipedCosmetics();
        }
        private void button_LoadBackup_MouseClick(object sender, MouseEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                InitialDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "Backups"),
                Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*",
                Title = "Choose fullProfile.json to use for backup"
            };
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog.FileName;
                string fileContent = File.ReadAllText(filePath);

                string fullProfile = FullProfile.Decrypt(fileContent);
                if (fullProfile == null)
                    CriticalError($"Failed to decrypt \"{filePath}\"!");
                if (fullProfile.IsJson() == false)
                    CriticalError($"\"{filePath}\" doesn't represent JSON!");

                if (FullProfile.Set(GetBhvrSession(), new FullProfile.S_Profile(JObject.Parse(fullProfile), GetFullProfileVersion())) == false)
                {
                    CriticalError("Failed to upload a backup fullProfile!");
                }
                else
                {
                    SoundPlayer soundPlayer = new SoundPlayer();
                    soundPlayer.Stream = Properties.Resources.SFX_Activate;
                    soundPlayer.Play();

                    SetFullProfileVersion(GetFullProfileVersion() + 1);
                }
            }
        }




        private void textBox_Head_Leave(object sender, EventArgs e)
        {
            UpdateEquipedCosmetics();
        }
        private void textBox_TorsoBody_Leave(object sender, EventArgs e)
        {
            UpdateEquipedCosmetics();
        }
        private void textBox_LegsWeapon_Leave(object sender, EventArgs e)
        {
            UpdateEquipedCosmetics();
        }
    }




    public class ApplicationPath
    {
        public static readonly string executableName = AppDomain.CurrentDomain.FriendlyName;            // "MyApplication.exe"
        public static readonly string executableFriendlyName = Process.GetCurrentProcess().ProcessName; // "MyApplication"


        public static readonly string executableDirectoryPath = AppDomain.CurrentDomain.BaseDirectory;        // "C:\Program Files\MyFolder"
        public static readonly string executablePath = Path.Combine(executableDirectoryPath, executableName); // "C:\Program Files\MyFolder\MyApplication.exe"


        public static readonly string charactersMapPath = Path.Combine(executableDirectoryPath, "data", "CharactersMap.json");
        public static readonly string charactersPortraitsDirectoryPath = Path.Combine(executableDirectoryPath, "data", "CharPortraits");
    }




    public class SessionData
    {
        private static string bhvrSession = null;
        public static string GetBhvrSession()
        {
            return bhvrSession;
        }
        public static void SetBhvrSession(string newBhvrSession)
        {
            bhvrSession = newBhvrSession.Replace("bhvrSession=", string.Empty);
        }


        private static FullProfile.S_Profile fullProfile;
        public static FullProfile.S_Profile GetFullProfile()
        {
            return fullProfile;
        }
        public static void SetFullProfile(FullProfile.S_Profile newFullProfile)
        {
            fullProfile = newFullProfile;
        }

        public static JObject GetFullProfileContent()
        {
            return fullProfile.content;
        }
        public static void SetFullProfileContent(JObject newContent)
        {
            fullProfile.content = newContent;
        }

        public static int GetFullProfileVersion()
        {
            return fullProfile.version;
        }
        public static void SetFullProfileVersion(int newVersion)
        {
            fullProfile.version = newVersion;
        }




        private static string selectedCharacterId = null;
        public static string GetSelectedCharacterId()
        {
            return selectedCharacterId;
        }
        public static void SetSelectedCharacterId(string newCharacterId)
        {
            selectedCharacterId = newCharacterId;
        }


        private static int selectedLoadoutSlot = 0;
        public static int GetSelectedLoadoutSlot()
        {
            return selectedLoadoutSlot;
        }
        public static void SetSelectedLoadoutSlot(int newLoadoutSlot)
        {
            selectedLoadoutSlot = newLoadoutSlot;
        }




        public struct S_CosmeticsPreset
        {
            public string head;
            public string torsoOrBody;
            public string legsOrWeapon;

            public S_CosmeticsPreset(string head, string torsoOrBody, string legsOrWeapon)
            {
                this.head = head;
                this.torsoOrBody = torsoOrBody;
                this.legsOrWeapon = legsOrWeapon;
            }
        }




        private static JObject charactersMap = null;
        public static JObject GetCharactersMap()
        { 
            return charactersMap; 
        }
        public static void SetCharactersMap(JObject newCharactersMap)
        {
            charactersMap = newCharactersMap;
        }


        public static Dictionary<int, string> characterSelectComboBoxMappings = new Dictionary<int, string>();
    }
}
