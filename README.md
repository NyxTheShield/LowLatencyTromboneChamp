# LowLatencyTromboneChamp
Mod that adds ASIO Support to Trombone Champ for low latency audio

## Requirements

- BepInEx 5 x64
- Trombone Champ v.1.04 onwards

## Install Instructions

- Extract both .dll files in your BepInEx's plugins folder, alongside the "LowLatencyAudioClips" folder. You should end with this structure

![image](https://user-images.githubusercontent.com/16619220/199308094-ccd46e4e-a036-4866-ada3-7ba11600ff6d.png)


## (Optional) Install FlexAsio
- Install FlexAsio and FlexAsio GUI, both included inside the "Asio Driver" folder

![image](https://user-images.githubusercontent.com/16619220/199308207-4a25beed-7418-4313-8b39-4ad4eea17ae5.png)

- Launch FlexAsio GUI and select your output device, (default settings should work fine, but if you are an advanced user you can try setting your buffer size and suggested latency settings). After that, click on "Save to Default FlexAsio.toml"

![image](https://user-images.githubusercontent.com/16619220/199308390-70307cb1-fbe3-4217-9435-6aa8ff6e4d5f.png)

- Launch the game, select FlexAsio as your driver and enjoy! 
