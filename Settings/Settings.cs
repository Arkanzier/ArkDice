using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Settings
{
    //This class is used to store and access system settings.

    public class Settings
    {
        //Stores the filepath to the directory where this program saves it's stuff to disk.
        internal string SaveFolder;
        //to do: function to process this whenever it comes in
        //create folders as appropriate
        //add/remove leading/trailing slashes as appropriate
        //make sure all slashes are / ?
        //more?

        //Stores the name we use for the folder all this program's stuff is stored in.
        //Generally not user editable.
        [JsonIgnore]
        internal string ProgramFolderName;
        //to do: decide what this should default to.

        //Stores the filename used for storing these settings.
        [JsonIgnore]
        internal string SettingsFilename;

        //Stores the filename used for storing a backup of the most recent settings file.
        [JsonIgnore]
        internal string SettingsBackupFilename;

        //Ideas for more settings:
        //Default character to load - string
        //If non-empty, will load this character (if present) automatically on startup
        //If it can't load them for some reason, complain and leave everything blank.
        //Select it via a dropdown, not text box.



        #region Constructors

        public Settings(bool loadFromDisk = true)
        {
            SaveFolder = "C:";
            //to do: change to current working directory?
            ProgramFolderName = "Simple Dice Roller";
            SettingsFilename = "Settings.dat";
            SettingsBackupFilename = "Settings-bak.dat";

            if (loadFromDisk)
            {
                LoadFromDisk();
            }
        }

        #endregion


        #region Getters for Filepaths

        //Functions that return the filepaths to various things
        //Some of these have private overloads that take a string as an argument. These are used as part of the process of moving the save folder.

        //Returns the path to the abilities library file.
        public string GetAbilitiesLibraryPath()
        {
            return GetBaseFolderPath() + "/dat/Abilities.dat";
        }

        //Returns the path to the folder where this stores non-settings information.
        public string GetBaseFolderPath()
        {
            return SaveFolder;
        }

        //Returns the filepath for the folder where backups of character files are moved.
        public string GetCharacterBackupFolderpath ()
        {
            return SaveFolder + "/Characters/Backups";
        }

        //Returns the expected filepath for where to save/load a character with a given name.
        public string GetCharacterFilepath (string charID)
        {
            string ret = "";

            string charFolder = GetCharacterFolderPath();

            string filename = GetFilenameForCharacter(charID);

            ret = charFolder + "/" + filename;

            return ret;
        }

        //Returns the filepath for the folder where characters are stored.
        public string GetCharacterFolderPath ()
        {
            return GetCharacterFolderPath(SaveFolder);
        }
        private string GetCharacterFolderPath (string location)
        {
            return location + "/Characters";
        }

        //Returns the filepath for the folder where the libraries are stored.
        public string GetDatFolderPath ()
        {
            return GetDatFolderPath(SaveFolder);
        }
        private string GetDatFolderPath (string location)
        {
            return location + "/dat";
        }

        //Returns the expected filename for a character based on that character's ID.
        //Usually this will be the character's ID with ".char" stuck onto the end, but this will also remove characters that aren't allowed in filenames.
        private string GetFilenameForCharacter(string charID)
        {
            string ret = "";

            //Do some logic here to remove problematic characters.
            char[] prohibitedChars = System.IO.Path.GetInvalidFileNameChars();

            string modified = charID;
            foreach (char c in prohibitedChars)
            {
                modified = modified.Replace(c.ToString(), "");
            }

            ret = modified + ".char";

            return ret;
        }

        //Returns the filepath to the folder where backups of library files are stored.
        public string GetLibraryBackupFolderpath()
        {
            return GetBaseFolderPath() + "/dat/Backups/";
        }

        //Returns the path to the spells library file.
        public string GetSpellsLibraryPath()
        {
            return GetBaseFolderPath() + "/dat/Spells.dat";
        }

        #endregion


        #region Functions for Saving and Loading This

        //Attempts to load a save file this has spat out.
        //Automatically accesses the same location as where SaveToDisk() saves to.
        private void LoadFromDisk()
        {
            //Check if the settings file exists.
            if (!File.Exists(GetSettingsSaveFilepath()))
            {
                //There isn't an existing save file, we'll just leave this on the defaults.
                //Put warning into log file?
                //MessageBox.Show("Couldn't find settings file.");
                return;
            }

            //There is an existing save file, we'll load it.

            //First we make sure it's not ginormous.
            FileInfo fileInfo = new FileInfo(GetSettingsSaveFilepath());

            //Returns the size of the file in bytes.
            long size = fileInfo.Length;

            //I don't know what the maximum reasonable file size for this is, but 10MB should be way more than enough.
            if (size > 1048576*10)
            {
                //This is too big, we're going to skip loading it.
                //complain to a log file
                return;
            }

            //Now we know that the file exists and is a reasonable size, so we load it.
            string text = File.ReadAllText(GetSettingsSaveFilepath());

            //MessageBox.Show("Loaded text: " + text);

            //We'll split the file up by line and then process it.
            string[] lines = text.Split("\r\n");
            foreach (var line in lines)
            {
                if (line.Trim().Length == 0)
                {
                    //This line is blank, we can move on.
                    continue;
                }

                //Split this line on the (first) equals sign (which we assume is present).
                string[] pieces = line.Split("=", 2);
                //this isn't splitting, why?
                if (pieces.Length < 2)
                {
                    //There's only one piece here. Did someone forget an = ?
                    continue;
                }
                else if (pieces.Length > 2)
                {
                    //We set a limit that should prevent this, how did this happen?
                    continue;
                }

                string index = pieces[0];
                string value = pieces[1];
                //MessageBox.Show("Split this line into index " + index + " and value " + value);
                //Consider adding some sanity checking to the values we pull out.

                //convert index to lower case so we can make this case insensitive?

                //If we get here, there are exactly 2 pieces to work with, which is the correct number.
                switch (index)
                {
                    case "SaveFolder":
                        this.SaveFolder = value;
                        //MessageBox.Show("Just set save folder to " + value);
                        break;
                    default:
                        //Complain to a log file?
                        break;
                }
            }

            return;
        }

        //Gets the filepath for where the backup of the settings file should be saved.
        private string GetSettingsSaveBackupFilepath()
        {
            string currentDirectory = Environment.CurrentDirectory;

            return currentDirectory + "/" + SettingsBackupFilename;
        }

        //Gets the filepath to the standard location where these settings should be saved.
        private string GetSettingsSaveFilepath()
        {
            string currentDirectory = Environment.CurrentDirectory;
            //MessageBox.Show("Current directory is " + currentDirectory);

            return currentDirectory + "/" + SettingsFilename;

            //return SaveFolder + "/" + ProgramFolderName + "/" + SettingsFilename;
            //to do: settings must be saved in the working directory, otherwise it won't know where to look to find the settings file.
            //look up how to get working directory.
        }

        //Saves the relevant data to disk in a standard location.
        public bool SaveToDisk()
        {
            string saveLocation = GetSettingsSaveFilepath();
            string backupLocation = GetSettingsSaveBackupFilepath();
            //MessageBox.Show("Going to save settings to " + saveLocation);

            string text = this.ToStringPrivate();

            //We don't need to make sure the destination folder exists, since we're just storing these in the working directory.

            if (File.Exists(backupLocation))
            {
                File.Delete(backupLocation);
            }

            //it's not getting the info to save, why?

            //Back up the file before we overwrite it.
            File.Move(saveLocation, backupLocation);

            //Save the current settings.
            File.WriteAllText(saveLocation, text);

            //Check if we were successful.
            if (File.Exists(saveLocation))
            {
                return true;
            } else
            {
                //complain to a log?
                return false;
            }
        }

        //Converts this object to a string.
        //This is private because I don't expect that anything other than this class' internal functions will ever use it.
        //We'll use an ini-style format rather than JSON because this is just going to be a handful of things with simple values.
        private string ToStringPrivate()
        {
            string ret = "";

            ret += "SaveFolder=" + SaveFolder;

            return ret;
        }

        //Updates this object to include any changes the user made, then writes the updated settings to disk.
        public void Update (Dictionary<string, string> data)
        {
            string SaveFolder = "";
            if (data.ContainsKey("SaveFolder"))
            {
                SaveFolder = data["SaveFolder"];
            }

            Update(SaveFolder);
        }
        public void Update (string SaveFolder = "")
        {
            if (SaveFolder == "")
            {
                //Default to current working directory.
            } else
            {
                if (SaveFolder != this.SaveFolder)
                {
                    UpdateSaveFolder(SaveFolder);
                    
                    bool resp = SaveToDisk();
                    if (resp)
                    {
                        //do something?
                    } else
                    {
                        //complain to a log file?
                    }
                }
            }
        }

        //Called when the save folder is changed.
        //Moves all files from the existing save folder, if there are any, over to the new one.
        private void UpdateSaveFolder (string newLocation)
        {
            //Before we start, we're just going to force all slashes to be /
            //This prevents the usage of \ as an escape character, but oh well too bad.
            newLocation = newLocation.Replace ('\\', '/');

            //Just do some quick error checking.
            if (newLocation == this.SaveFolder)
            {
                //These are identical, we have nothing to do.
                //complain to a log file?
                return;
            }

            //check if the new location is somewhere we're not allowed to access.
            //is there an easy way to check if we're allowed to write somewhere?
            //definitely blacklist C:/Windows

            //We'll leave room for the possibility of case-sensitive file structures.
            bool shouldMove = true;
            if (String.Equals(newLocation, this.SaveFolder, StringComparison.OrdinalIgnoreCase))
            {
                shouldMove = false;
                //These two filepaths are the same, aside from capitalization.
                //Windows uses a case-insensitive file system, which would mean that they point to the same spot and we can safely update the string without moving any files around.
                //We'll leave room for the possibility of someone running this in an OS with a case-sensitive file system and investigate the possibility that they might be different spots.

                //If the new location is missing some files/folders (and the old location isn't), then they're definitely different.
                //We don't learn anything new if both or neither of them has some file/folder.
                if (File.Exists(GetDatFolderPath(SaveFolder)) && !File.Exists(GetDatFolderPath(newLocation)))
                {
                    shouldMove = true;
                }
                if (!shouldMove && File.Exists(GetCharacterFolderPath(SaveFolder)) && !File.Exists(GetCharacterFolderPath(newLocation)))
                {
                    shouldMove = true;
                }
                
                //If shouldMove is still false, both places have identical sets of files.
                //Eventually we'll want to check file modification times and/or contents.
                //For now, we'll just assume they're the same.
            }

            if (shouldMove)
            {
                //Move the files.
                Directory.Move(GetDatFolderPath(SaveFolder), GetDatFolderPath(newLocation));
                Directory.Move(GetCharacterFolderPath(SaveFolder), GetCharacterFolderPath(newLocation));
            }

            //Make sure the files are in the new location.
            //...

            this.SaveFolder = newLocation;

            //return true/false?
        }

        #endregion

    }
}
