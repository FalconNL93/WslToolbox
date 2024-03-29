﻿using CommunityToolkit.Mvvm.ComponentModel;

namespace WslToolbox.UI.Core.Models;

public class Distribution : ObservableRecipient
{
    public const string StateRunning = "Running";
    public const string StateStopped = "Stopped";
    public const string StateAvailable = "Stopped";
    public const string StateBusy = "Busy";
    private bool _isAvailable;

    private string _state;

    public bool IsDefault { get; set; }
    public bool IsInstalled { get; set; }
    public string Name { get; set; }

    public bool IsAvailable
    {
        get => _isAvailable;
        set => SetProperty(ref _isAvailable, value);
    }

    public string State
    {
        get => _state;
        set
        {
            IsAvailable = value != StateBusy;
            SetProperty(ref _state, value);
        }
    }

    public int Version { get; set; }
    public string Guid { get; set; }
    public string BasePath { get; set; }
    public string BasePathLocal { get; set; }
    public int DefaultUid { get; set; }
    public long Size { get; set; }

    public Distribution Clone()
    {
        return (Distribution) MemberwiseClone();
    }
}