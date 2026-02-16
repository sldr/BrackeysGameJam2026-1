using Godot;
using System;

public partial class OptRes : Resource
{
	public bool isSettingsSaved = false;

	[ExportCategory("Stored Settings")]

	[ExportGroup("Stored Volume Settings")]
	[Export(PropertyHint.Range, "-80,6,0.5")] public float MainVolumeDb = 0;
	[Export(PropertyHint.Range, "-80,6,0.5")] public float MusicVolumeDb = 0;
	[Export(PropertyHint.Range, "-80,6,0.5")] public float SFXVolumeDb = 0;

	[Export] public bool MainVolMuted = false;
	[Export] public bool MusicVolMuted = false;
	[Export] public bool SFXVolMuted = false;

	[ExportGroup("Stored Gameplay Settings")]
	[Export] public int StoredMovementSpeed = 200;
	[Export] public int StoredBallSpeed = 300;

	[ExportCategory("Default Settings")]
	[ExportSubgroup("Volume Defaults")]
	[Export(PropertyHint.Range, "-80,6,0.5")] public float DefaultMainVolumeDb = 0;
	[Export(PropertyHint.Range, "-80,6,0.5")] public float DefaultMusicVolumeDb = 0;
	[Export(PropertyHint.Range, "-80,6,0.5")] public float DefaultSFXVolumeDb = 0;

	[Export] public bool DefaultMainVolMuted = false;
	[Export] public bool DefaultMusicVolMuted = false;
	[Export] public bool DefaultSFXVolMuted = false;
	
	[ExportSubgroup("Gameplay Defaults")]

	[Export] public int DefaultMovementSpeed = 200;
	[Export] public int DefaultBallSpeed = 300;
}

