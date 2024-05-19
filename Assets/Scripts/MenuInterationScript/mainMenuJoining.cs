using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class mainMenuJoining : MonoBehaviour
{
    [SerializeField]
    PlayerInputManager PIM;

    // Start is called before the first frame update
    void Start()
    {
        TryGetComponent<PlayerInputManager>(out PIM);

        PIM.JoinPlayer(0);
        PIM.JoinPlayer(1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
