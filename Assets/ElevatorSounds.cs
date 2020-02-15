using System;
using System.Collections;
using System.Collections.Generic;
using Elevator;
using Sounds;
using UnityEngine;

public class ElevatorSounds : MonoBehaviour
{
    public Sound[] elevatorSounds;
    private ElevatorController m_Controller;

    // private helper state holders
    private bool m_Started = false;
    private bool m_Stopped = false;
    private bool m_PlayingMoveSound = false;
    private bool m_DoorOpenPlayed = false;
    private bool m_DoorClosePlayed = false;

    private void Start()
    {
        m_Controller = GetComponent<ElevatorController>();
        foreach (var sound in elevatorSounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loop;
            sound.source.spatialBlend = 0.8f;
        }
    }

    private void Update()
    {
        ControlElevatorSoundsBasedOnStatus();
    }

    public void Play(string soundName)
    {
        var s = Array.Find(elevatorSounds, sound => sound.name == soundName);
        s?.source.Play();
    }

    public void Stop(string soundName)
    {
        var s = Array.Find(elevatorSounds, sound => sound.name == soundName);
        s?.source.Stop();
    }

    private void ControlElevatorSoundsBasedOnStatus()
    {
        // based on actions decide which source track to play
        if (m_Controller.inMove)
        {
            // start elevator
            if (!m_Started)
            {
                Play("Start");
                m_Started = true;
                m_Stopped = false;
            }

            if (!m_PlayingMoveSound)
            {
                Play("Move");
                m_PlayingMoveSound = true;
            }
        }
        else
        {
            // stop elevator
            if (!m_Stopped)
            {
                Stop("Move");
                m_PlayingMoveSound = false;

                Play("Stop");
                m_Stopped = true;
                m_Started = false;
            }
        }

        // door sounds
        if (m_Controller.doorsClosing)
        {
            if (m_DoorClosePlayed) return;
            m_DoorClosePlayed = true;
            m_DoorOpenPlayed = false;
            Play("DoorClose");
        }
        else
        {
            // if doors are opened
            if (m_Controller.doorsClosed) return;
            if (m_DoorOpenPlayed) return;
            m_DoorClosePlayed = false;
            m_DoorOpenPlayed = true;
            Play("DoorOpen");
        }
    }
}