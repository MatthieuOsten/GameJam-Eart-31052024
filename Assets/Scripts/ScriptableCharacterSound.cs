using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSoundUnit", menuName = "Scriptable/Sound/Unit")]
public class ScriptableSoundUnit : ScriptableObject
{
    [Header("MOVEMENT")]
    /// <summary>
    /// Make sound to begin of walking
    /// </summary>
    [SerializeField] private string _soundWalk;
    /// <summary>
    /// Make sound to jump
    /// </summary>
    [SerializeField] private string _soundJump;

    [Header("STATE")]
    /// <summary>
    /// Reaction of this dead
    /// </summary>
    [SerializeField] private string _soundDead;

    [Header("SHOOT")]
    /// <summary>
    /// Make sound to shoot
    /// </summary>
    [SerializeField] private string _soundShoot;

    [Header("REACTION")]
    /// <summary>
    /// React to takeDamage
    /// </summary>
    [SerializeField] private string _soundDamaged;

    /// <summary>
    /// Meta reaction of paused game
    /// </summary>
    [SerializeField] private string _soundPaused;

}
