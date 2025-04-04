using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIPlayerJumpTracker : MonoBehaviour
{
    public List<Image> jumpIcons; // Assign 5 UI Images in Inspector
    public Sprite activeJumpSprite;   // Used jump icon
    public Sprite inactiveJumpSprite; // Available jump icon

    private PlayerPhysicsController playerPhysicsController; // Reference to player movement/jumping script

    void Start()
    {
        playerPhysicsController = FindObjectOfType<PlayerPhysicsController>(); // Auto-find player script
        UpdateUIPlayerJumpTracker();
    }

    void Update()
    {
        if (playerPhysicsController != null)
        {
            UpdateUIPlayerJumpTracker();
        }
    }

    private void UpdateUIPlayerJumpTracker()
    {
        int jumpsUsed = playerPhysicsController.GetJumpsUsed();
        int maxJumps = playerPhysicsController.getMaxJumps();

        for (int i = 0; i < maxJumps; i++)
        {
            jumpIcons[i].sprite = (i < jumpsUsed) ? activeJumpSprite : inactiveJumpSprite;
        }
    }
}