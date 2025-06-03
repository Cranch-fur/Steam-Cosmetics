// #define STEAM_AUTH

using CranchyLib.Networking;
using Newtonsoft.Json.Linq;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Media;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SteamCosmetics
{
    public partial class Form_Main : Form
    {
        private struct S_CustomizationPreset
        {
            public string head;
            public string torsoOrBody;
            public string legsOrWeapon;

            public S_CustomizationPreset(string head, string torsoOrBody, string legsOrWeapon)
            {
                this.head = head;
                this.torsoOrBody = torsoOrBody;
                this.legsOrWeapon = legsOrWeapon;
            }
        }




        private const int applicationId = 381210;


        private JObject jsonCharactersMap = null;
        private Dictionary<int, string> characterSelectComboBoxMappings = new Dictionary<int, string>();
        private string characterSelectSelectedCharacterId = null;
        private int characterSelectSelectedLoadoutSlot = 0;


        private string bhvrSession = null;


        private FullProfile.S_Profile fullProfile;
        private JObject jsonFullProfile;




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




        private S_CustomizationPreset GetEquipedCosmetics(string characterId)
        {
            foreach (var customizationPreset in jsonFullProfile["characterCustomizationPresets"])
            {
                if ((string)customizationPreset["characterId"] == characterId)
                {
                    return new S_CustomizationPreset((string)customizationPreset["presets"][characterSelectSelectedLoadoutSlot]["head"], (string)customizationPreset["presets"][characterSelectSelectedLoadoutSlot]["torsoOrBody"], (string)customizationPreset["presets"][characterSelectSelectedLoadoutSlot]["legsOrWeapon"]);
                }
            }

            return new S_CustomizationPreset(null, null, null);
        }
        private void UpdateSelectedCharacterPreview()
        {
            pictureBox_CharacterPortrait.Image = Image.FromFile((string)jsonCharactersMap[characterSelectSelectedCharacterId]["charPortrait"]);

            S_CustomizationPreset equipedCosmetics = GetEquipedCosmetics(characterSelectSelectedCharacterId);
            textBox_Head.Text = equipedCosmetics.head;
            textBox_TorsoBody.Text = equipedCosmetics.torsoOrBody;
            textBox_LegsWeapon.Text = equipedCosmetics.legsOrWeapon;
        }
        private void SetEquipedCosmetics(string characterId, S_CustomizationPreset newPreset, bool commitChanges = false)
        {
            JToken charactersCustomizationPresets = jsonFullProfile["characterCustomizationPresets"];

            foreach (var customizationPreset in charactersCustomizationPresets)
            {
                if ((string)customizationPreset["characterId"] == characterId)
                {
                    JToken customizations = customizationPreset["presets"][characterSelectSelectedLoadoutSlot];
                    customizations["head"] = newPreset.head;
                    customizations["torsoOrBody"] = newPreset.torsoOrBody;
                    customizations["legsOrWeapon"] = newPreset.legsOrWeapon;

                    break;
                }
            }

            if (commitChanges == true)
            {
                if (FullProfile.Set(bhvrSession, jsonFullProfile.ToString(), fullProfile.version) == false)
                {
                    CriticalError("Failed to upload a new fullProfile!");
                }
                else
                {
                    SoundPlayer soundPlayer = new SoundPlayer();
                    soundPlayer.Stream = Properties.Resources.SFX_Activate;
                    soundPlayer.Play();

                    fullProfile.version++;
                }
            }
        }




        public Form_Main()
        {
            InitializeComponent();
        }
        private void Form_Main_Load(object sender, EventArgs e)
        {
            if (File.Exists("steam_api64.dll") == false)
            {
                CriticalError("Failed to find \"steam_api64.dll\"!");
            }


            if (File.Exists("Data\\CharactersMap.json") == false)
            {
                CriticalError("Failed to find \"CharactersMap.json\"!");
            }


            string charactersMap = File.ReadAllText("Data\\CharactersMap.json");
            if (charactersMap.IsJson() == false)
            {
                CriticalError("Failed to read \"CharactersMap.json\"!");
            }
            else
            {
                jsonCharactersMap = JObject.Parse(charactersMap);
            }


            if (IsSteamRunning() == false)
            {
                CriticalError("Steam Client is not running!");
            }


            SteamClient.Init(applicationId);
            if (SteamApps.IsAppInstalled(applicationId) == false)
            {
                CriticalError("Dead By Daylight game isn't installed!");
            }


            if (IsGameRunning() == true)
            {
                CriticalError("Dead By Daylight game shouldn't be running!");
            }
            else 
            {
                Task.Run(() => GameProcessLookupHandler());
            }


#if STEAM_AUTH
            AuthTicket authSessionTicket = SteamUser.GetAuthSessionTicket();
            string authSessionTicketString = BitConverter.ToString(authSessionTicket.Data).Replace("-", string.Empty)
                                                                                          .TrimEnd('0');

            List<string> headers = new List<string>
            {
                "x-kraken-content-secret-key: N+r8gZ47S2ZDQ2nurlp7FbCwe+gB6OtpAftTK9Zf5Cs="
            };
            var gameLoginResponse = Networking.Post($"https://steam.live.bhvrdbd.com/api/v1/auth/provider/steam/loginV2?token={authSessionTicketString}", headers);
            if (gameLoginResponse.statusCode != Networking.E_StatusCode.OK)
            {
                CriticalError($"Failed to proceed with login request! [{gameLoginResponse.statusCode}]");
            }


            foreach (string responseHeader in gameLoginResponse.headers)
            {
                if (responseHeader.StartsWith("Set-Cookie:"))
                {
                    bhvrSession = responseHeader.Split('=')[1];
                }
            }
            if (bhvrSession == null)
            {
                CriticalError("Failed to retrieve bhvrSession!");
            }
#else
            if (File.Exists("bhvrSession.txt") == false)
                CriticalError("\"bhvrSession.txt\" file is missing!");

            string bhvrSessionFileContents = File.ReadAllText("bhvrSession.txt");
            if (string.IsNullOrEmpty(bhvrSessionFileContents))
                CriticalError("\"bhvrSession.txt\" file is empty!");


            List<string> headers = new List<string>
            {
                $"Cookie: bhvrSession={bhvrSessionFileContents}"
            };
            var gameConfigResponse = Networking.Get("https://steam.live.bhvrdbd.com/api/v1/config", headers);
            if (gameConfigResponse.statusCode != Networking.E_StatusCode.OK)
                CriticalError("bhvrSession is invalid or game server are down!");


            bhvrSession = bhvrSessionFileContents;
#endif


            fullProfile = FullProfile.Get(bhvrSession);
            if (fullProfile.version == -1)
            {
                CriticalError("Failed to retrieve fullProfile!");
            }


            jsonFullProfile = JObject.Parse(fullProfile.content);
            if (jsonFullProfile.ContainsKey("characterCustomizationPresets") == false)
            {
                CriticalError("Failed to retrieve characterCustomizationPresets!");
            }


            foreach (var character in jsonCharactersMap)
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
            characterSelectSelectedCharacterId = characterSelectComboBoxMappings[comboBox_CharacterSelect.SelectedIndex];
            UpdateSelectedCharacterPreview();
        }
        private void trackBar_LoadoutSlot_Scroll(object sender, EventArgs e)
        {
            label_LoadoutSlot.Text = $"Loadout Slot #{trackBar_LoadoutSlot.Value}";
            characterSelectSelectedLoadoutSlot = trackBar_LoadoutSlot.Value - 1;

            UpdateSelectedCharacterPreview();
        }




        private void button_Update_MouseClick(object sender, MouseEventArgs e)
        {
            SetEquipedCosmetics(characterSelectSelectedCharacterId, new S_CustomizationPreset(textBox_Head.Text, textBox_TorsoBody.Text, textBox_LegsWeapon.Text), true);
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

                if (FullProfile.Set(bhvrSession, fileContent, fullProfile.version) == false)
                {
                    CriticalError("Failed to upload a backup fullProfile!");
                }
                else
                {
                    SoundPlayer soundPlayer = new SoundPlayer();
                    soundPlayer.Stream = Properties.Resources.SFX_Activate;
                    soundPlayer.Play();

                    fullProfile.version++;
                }
            }
        }




        private void textBox_Head_Leave(object sender, EventArgs e)
        {
            SetEquipedCosmetics(characterSelectSelectedCharacterId, new S_CustomizationPreset(textBox_Head.Text, textBox_TorsoBody.Text, textBox_LegsWeapon.Text));
        }
        private void textBox_TorsoBody_Leave(object sender, EventArgs e)
        {
            SetEquipedCosmetics(characterSelectSelectedCharacterId, new S_CustomizationPreset(textBox_Head.Text, textBox_TorsoBody.Text, textBox_LegsWeapon.Text));
        }
        private void textBox_LegsWeapon_Leave(object sender, EventArgs e)
        {
            SetEquipedCosmetics(characterSelectSelectedCharacterId, new S_CustomizationPreset(textBox_Head.Text, textBox_TorsoBody.Text, textBox_LegsWeapon.Text));
        }
    }
}
