# What is this?
This program converts any given png, jpg, or gif image file into the Genshin Impact Namecard format, then you can combine those in any order into a singular merged mod that overwrites any namecard you want, up until 6.3 (Luna 4). I have not tested all 225 namecards to make 100% sure they are correct, but they should be all correct except for 2 that I know have issues:
* Genshin Impact: A New World - This is the first namecard you get and the top banner in the namecard selection screen for this one is actually 2 different hashes combined while every other namecard is a single hash. I am not going to put in the time to figure out how to make that single instance work, so the top banner for this namecard is incorrect and will stay that way unless someone else wants to figure it out.
* Fontaine: Redemption - This one slightly different dimensions, 843 pixels vs the standard 840 pixels wide, so I just didn't add it to the list since I would have to hard code in the hash that can change in the future for a special case.

The actual mod format I took for the .ini files is from [papadude86](https://gamebanana.com/members/2721194) on Gamebanana (WARNING: NSFW), so all of their mods should work if you want to make one big namecard megatoggle with parts of those as well. I would recommend renaming the mod folders from those to be unique if you do that. The UI is heavily inspired from gui_collect because I'm bad at making a good UI and the gui_collect UI fits what I needed for this. It's not pretty, but it works.

## How does it work?

1) Open the program and select the folder where you want everything to be saved using the top right Folder button, including the image conversions as well as the final mod by clicking the folder icon.
2) If you have your own images or gifs to convert, simply click the "Convert Image" button on the left and then select your images. Then, click the Convert button on the right side to begin the conversion.
3) If you want to change the toggle and pause keybinds, then click on the "Settings" button on the bottom left. Click one of the textboxes and just press the keybind you want, then it will be updated. Randomized Login is exactly what it says; it will pick a random namecard to display on login if it's on.
4) Click on the "Create Mod" button on the left side to go back to the page where you actually create the mod. You may need to click the Refresh button next to the Folder button in the top right if you just converted some images. It does not automatically refresh the list of folders when folders are added or removed. Use the arrow buttons to swap the mod folders in and out of the final merged collection. The up and down arrows simply let you decide order of the mods if you wish to.
5) Once everything looks good, simply click the "Create" button on the "Create Mod" page and your namecard mod should be finished.

## The hashes are wrong and/or changed

This where the "Update Hashes" page comes into play. If I'm not updating this anymore, which I made this intending to be a one and done that ended up being a public release. Perform a [hash dump](https://leotorrez.github.io/modding/guides/hunting) while you have the namecard you are trying to find currently selected as shown below. While that tutorial link mentions XXMI, the process is the exact same on old 3dmigoto if you're on that due to being disconnected. It should not take more than a few seconds depending on your hard drive speed.<img width="1920" height="974" alt="namecardscreen" src="https://github.com/user-attachments/assets/c09254ec-2420-4372-ae3e-7c69cd4101c9" />
Then, go into the newly created frame dump folder to search for ".dds" and change your View to "Extra Large Icons" to make it easier to find the 3 different files you want. This is actually how you dump any UI element in GI as far as I know.
<img width="1600" height="860" alt="preview1" src="https://github.com/user-attachments/assets/b0948d46-42d5-46a2-9519-971c372c59d4" />
You can see the "Main Hash" that the program wants at the bottom right. The hash value is the first part of the  filename, d3768864 for this case. The other two parts look like below and you can see their hashes in the filename as well:

Preview Hash: <img width="274" height="289" alt="previewbanner" src="https://github.com/user-attachments/assets/e07ae124-2e9f-4361-92eb-91e19ab752db" />

Banner Hash: <img width="293" height="247" alt="bannerhash" src="https://github.com/user-attachments/assets/3ee42f36-2ee3-4de6-b906-3ef9523ebf6f" />

If the namecard was released after 6.3/Luna 4 and isn't in the program, just click New on the "Update Hashes" page and put in the relevant hashes and name using the method above, otherwise find the namecard in the drop down list and Edit it as needed if the hashes changed on an update.

## Future Updates
I might add more Genshin Impact UI elements like a character's skill, burst, etc, icons to this since it should be pretty trivial depending on how much free time I have and if they are all standardized between characters. It took me over 6 hours to get and input all of the namecard hashes. I'm not doing that again if I don't have to, so no promises.
