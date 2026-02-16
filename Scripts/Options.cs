using Godot;
using System;

public partial class Options : Node2D
{
	// Current Audio Settings
	public float MainVolumeDb = 0;
	public float MusicVolumeDb = 0;
	public float SFXVolumeDb = 0;
	public bool MainVolMuted = false;
	public bool MusicVolMuted = false;
	public bool SFXVolMuted = false;
	// Current Gameplay Settings
	public int MovementSpeed = 200;
	public int BallSpeed = 300;

	private OptRes optRes = GD.Load<OptRes>("res://Options.tres");

	///<summary> HOW TO ADD NEW SETTINGS:
	/// Add a variable to the Options script to hold the "current value" of the setting.
	/// Add two variables to the Options Resource (OptRes.cs) to hold the "saved value" and a "default value".
	/// Initialize it in ready by loading the "saved value" from options resource then applying the "saved value" or "default value" to UI.
	/// {SIDE NOTE}
	/// 	Initialize the "default value" and "saved value" where ever else appropriate: 
	/// 	Load the default and stored settings in other code by using "OptRes optRes = GD.Load<OptRes>("res://Options.tres");" 
	///		Then access the variable using "optRes.VariableName".
	/// 	If it is a gameplay object then initialize in objects gameplay code. 
	///		If it is audio initialize on audio singleton.
	/// {/SIDE NOTE}
	/// Add the UI setting changed and reset signals.
	/// On UI Setting Changed, only update the "current value".
	/// On Reset, set the UI and the "current value" to the "default value" defined in the options resource.
	/// On Save, update the options resource "saved value" to the "current value" and save the resource.
	/// On Exit. Change scene to main menu.
	/// </summary>
	public override void _Ready()
	{
		if (optRes.isSettingsSaved)
		{
			// Load saved settings
			MainVolumeDb = optRes.MainVolumeDb;
			MusicVolumeDb = optRes.MusicVolumeDb;
			SFXVolumeDb = optRes.SFXVolumeDb;
			MainVolMuted = optRes.MainVolMuted;
			MusicVolMuted = optRes.MusicVolMuted;
			SFXVolMuted = optRes.SFXVolMuted;
			MovementSpeed = optRes.StoredMovementSpeed;
			BallSpeed = optRes.StoredBallSpeed;
			// Apply all saved states to audio server
			AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex("Master"), MainVolumeDb);
			AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex("Music"), MusicVolumeDb);
			AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex("Sfx"), SFXVolumeDb);
			AudioServer.SetBusMute(AudioServer.GetBusIndex("Master"), MainVolMuted);
			AudioServer.SetBusMute(AudioServer.GetBusIndex("Music"), MusicVolMuted);
			AudioServer.SetBusMute(AudioServer.GetBusIndex("Sfx"), SFXVolMuted);
			// Apply all saved states to sliders
			GetNode<Slider>("Menu/AspectRatioContainer/CenterContainer/PanelContainer/MarginContainer/VBoxContainer/TabContainer/Audio/VBoxContainer/MainVol/MainVolSlider").Value = MainVolumeDb;
			GetNode<Slider>("Menu/AspectRatioContainer/CenterContainer/PanelContainer/MarginContainer/VBoxContainer/TabContainer/Audio/VBoxContainer/MusicVol/MusicVolSlider").Value = MusicVolumeDb;
			GetNode<Slider>("Menu/AspectRatioContainer/CenterContainer/PanelContainer/MarginContainer/VBoxContainer/TabContainer/Audio/VBoxContainer/SfxVol/SfxVolSlider").Value = SFXVolumeDb;
			GetNode<BaseButton>("Menu/AspectRatioContainer/CenterContainer/PanelContainer/MarginContainer/VBoxContainer/TabContainer/Audio/VBoxContainer/MainVol/MainVolMute").ButtonPressed = MainVolMuted;
			GetNode<BaseButton>("Menu/AspectRatioContainer/CenterContainer/PanelContainer/MarginContainer/VBoxContainer/TabContainer/Audio/VBoxContainer/MusicVol/MusicVolMute").ButtonPressed = MusicVolMuted;
			GetNode<BaseButton>("Menu/AspectRatioContainer/CenterContainer/PanelContainer/MarginContainer/VBoxContainer/TabContainer/Audio/VBoxContainer/SfxVol/SfxVolMute").ButtonPressed = SFXVolMuted;
			GetNode<SpinBox>("Menu/AspectRatioContainer/CenterContainer/PanelContainer/MarginContainer/VBoxContainer/TabContainer/Gameplay/VBoxContainer/HBoxContainer/MovementSpeedBox").Value = MovementSpeed;
			GetNode<SpinBox>("Menu/AspectRatioContainer/CenterContainer/PanelContainer/MarginContainer/VBoxContainer/TabContainer/Gameplay/VBoxContainer/HBoxContainer2/BallSpeedBox").Value = BallSpeed;
		}
		else
		{
			// Load default settings
			MainVolumeDb = optRes.DefaultMainVolumeDb;
			MusicVolumeDb = optRes.DefaultMusicVolumeDb;
			SFXVolumeDb = optRes.DefaultSFXVolumeDb;
			MainVolMuted = optRes.DefaultMainVolMuted;
			MusicVolMuted = optRes.DefaultMusicVolMuted;
			SFXVolMuted = optRes.DefaultSFXVolMuted;
			MovementSpeed = optRes.DefaultMovementSpeed;
			BallSpeed = optRes.DefaultBallSpeed;
			// Apply all default states to audio server
			AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex("Master"), MainVolumeDb);
			AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex("Music"), MusicVolumeDb);
			AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex("Sfx"), SFXVolumeDb);
			AudioServer.SetBusMute(AudioServer.GetBusIndex("Master"), MainVolMuted);
			AudioServer.SetBusMute(AudioServer.GetBusIndex("Music"), MusicVolMuted);
			AudioServer.SetBusMute(AudioServer.GetBusIndex("Sfx"), SFXVolMuted);
			// Apply all default states to sliders
			GetNode<Slider>("Menu/AspectRatioContainer/CenterContainer/PanelContainer/MarginContainer/VBoxContainer/TabContainer/Audio/VBoxContainer/MainVol/MainVolSlider").Value = MainVolumeDb;
			GetNode<Slider>("Menu/AspectRatioContainer/CenterContainer/PanelContainer/MarginContainer/VBoxContainer/TabContainer/Audio/VBoxContainer/MusicVol/MusicVolSlider").Value = MusicVolumeDb;
			GetNode<Slider>("Menu/AspectRatioContainer/CenterContainer/PanelContainer/MarginContainer/VBoxContainer/TabContainer/Audio/VBoxContainer/SfxVol/SfxVolSlider").Value = SFXVolumeDb;
			GetNode<BaseButton>("Menu/AspectRatioContainer/CenterContainer/PanelContainer/MarginContainer/VBoxContainer/TabContainer/Audio/VBoxContainer/MainVol/MainVolMute").ButtonPressed = MainVolMuted;
			GetNode<BaseButton>("Menu/AspectRatioContainer/CenterContainer/PanelContainer/MarginContainer/VBoxContainer/TabContainer/Audio/VBoxContainer/MusicVol/MusicVolMute").ButtonPressed = MusicVolMuted;
			GetNode<BaseButton>("Menu/AspectRatioContainer/CenterContainer/PanelContainer/MarginContainer/VBoxContainer/TabContainer/Audio/VBoxContainer/SfxVol/SfxVolMute").ButtonPressed = SFXVolMuted;
			GetNode<SpinBox>("Menu/AspectRatioContainer/CenterContainer/PanelContainer/MarginContainer/VBoxContainer/TabContainer/Gameplay/VBoxContainer/HBoxContainer/MovementSpeedBox").Value = MovementSpeed;
			GetNode<SpinBox>("Menu/AspectRatioContainer/CenterContainer/PanelContainer/MarginContainer/VBoxContainer/TabContainer/Gameplay/VBoxContainer/HBoxContainer2/BallSpeedBox").Value = BallSpeed;
		}
	}
#region Audio Setting Signals
	private void _on_main_vol_slider_value_changed(float value)
	{
		MainVolumeDb = value;
		AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex("Master"), MainVolumeDb);
	}

	private void _on_sfx_vol_slider_value_changed(float value)
	{
		SFXVolumeDb = value;
		AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex("Sfx"), SFXVolumeDb);
	}

	private void _on_music_vol_slider_value_changed(float value)
	{
		MusicVolumeDb = value;
		AudioServer.SetBusVolumeDb(AudioServer.GetBusIndex("Music"), MusicVolumeDb);
	}
	private void _on_main_vol_mute_toggled(bool pressed)
	{
		MainVolMuted = pressed;
		AudioServer.SetBusMute(AudioServer.GetBusIndex("Master"), MainVolMuted);
	}
	private void _on_music_vol_mute_toggled(bool pressed)
	{
		MusicVolMuted = pressed;
		AudioServer.SetBusMute(AudioServer.GetBusIndex("Music"), MusicVolMuted);
	}
	private void _on_sfx_vol_mute_toggled(bool pressed)
	{
		SFXVolMuted = pressed;
		AudioServer.SetBusMute(AudioServer.GetBusIndex("Sfx"), SFXVolMuted);
	}
	private void _on_main_vol_reset_pressed()
	{
		MainVolumeDb = optRes.DefaultMainVolumeDb;
		MainVolMuted = optRes.DefaultMainVolMuted;
		GetNode<Slider>("Menu/AspectRatioContainer/CenterContainer/PanelContainer/MarginContainer/VBoxContainer/TabContainer/Audio/VBoxContainer/MainVol/MainVolSlider").Value = MainVolumeDb;
		GetNode<BaseButton>("Menu/AspectRatioContainer/CenterContainer/PanelContainer/MarginContainer/VBoxContainer/TabContainer/Audio/VBoxContainer/MainVol/MainVolMute").ButtonPressed = MainVolMuted;
	}
	private void _on_music_vol_reset_pressed()
	{
		MusicVolumeDb = optRes.DefaultMusicVolumeDb;
		MusicVolMuted = optRes.DefaultMusicVolMuted;
		GetNode<Slider>("Menu/AspectRatioContainer/CenterContainer/PanelContainer/MarginContainer/VBoxContainer/TabContainer/Audio/VBoxContainer/MusicVol/MusicVolSlider").Value = MusicVolumeDb;
		GetNode<BaseButton>("Menu/AspectRatioContainer/CenterContainer/PanelContainer/MarginContainer/VBoxContainer/TabContainer/Audio/VBoxContainer/MusicVol/MusicVolMute").ButtonPressed = MusicVolMuted;
	}
	private void _on_sfx_vol_reset_pressed()
	{
		SFXVolumeDb = optRes.DefaultSFXVolumeDb;
		SFXVolMuted = optRes.DefaultSFXVolMuted;
		GetNode<Slider>("Menu/AspectRatioContainer/CenterContainer/PanelContainer/MarginContainer/VBoxContainer/TabContainer/Audio/VBoxContainer/SfxVol/SfxVolSlider").Value = SFXVolumeDb;
		GetNode<BaseButton>("Menu/AspectRatioContainer/CenterContainer/PanelContainer/MarginContainer/VBoxContainer/TabContainer/Audio/VBoxContainer/SfxVol/SfxVolMute").ButtonPressed = SFXVolMuted;
	}
#endregion
#region Gameplay Setting Signals
	private void _on_movement_speed_box_value_changed(double value)
	{
		MovementSpeed = (int)value;
	}
	private void _on_ball_speed_box_value_changed(double value)
	{
		BallSpeed = (int)value;
	}
	private void _on_movement_speed_reset_pressed()
	{
		MovementSpeed = optRes.DefaultMovementSpeed;
		GetNode<SpinBox>("Menu/AspectRatioContainer/CenterContainer/PanelContainer/MarginContainer/VBoxContainer/TabContainer/Gameplay/VBoxContainer/HBoxContainer/MovementSpeedBox").Value = MovementSpeed;
	}
	private void _on_ball_speed_reset_pressed()
	{
		BallSpeed = optRes.DefaultBallSpeed;
		GetNode<SpinBox>("Menu/AspectRatioContainer/CenterContainer/PanelContainer/MarginContainer/VBoxContainer/TabContainer/Gameplay/VBoxContainer/HBoxContainer2/BallSpeedBox").Value = BallSpeed;
	}
	
#endregion
	private void _on_save_pressed()
	{
		optRes.MainVolumeDb = MainVolumeDb;
		optRes.MusicVolumeDb = MusicVolumeDb;
		optRes.SFXVolumeDb = SFXVolumeDb;
		optRes.MainVolMuted = MainVolMuted;
		optRes.MusicVolMuted = MusicVolMuted;
		optRes.SFXVolMuted = SFXVolMuted;
		optRes.StoredMovementSpeed = MovementSpeed;
		optRes.StoredBallSpeed = BallSpeed;
		ResourceSaver.Save(optRes, "res://Options.tres");
		optRes.isSettingsSaved = true;
	}

	public void _on_exit_pressed()
	{
		GetTree().ChangeSceneToFile("res://Main.tscn");
	}
}
