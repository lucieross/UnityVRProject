using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.InputSystem;
using System.Collections; 

public class Hand_Animator : MonoBehaviour
{
    [SerializeField] private NearFarInteractor nearFarInteractor;
    [SerializeField] private SkinnedMeshRenderer handMesh;
    [SerializeField] private GameObject handArmature;
    [SerializeField] private InputActionReference selectActionRef;
    [SerializeField] private InputActionReference activateActionRef;
    [SerializeField] private Animator handAnimator;
    [SerializeField] private float actionDelay = 0.3f;

    private static readonly int activateAnim = Animator.StringToHash("activate");
    private static readonly int selectAnim = Animator.StringToHash("select");
    

    private bool emptyHand = true;

    private void Awake()
    {
        emptyHand = true;
        nearFarInteractor.selectEntered.AddListener(OnGrab);
        nearFarInteractor.selectExited.AddListener(OnRelease);
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        emptyHand = false; 
        StartCoroutine(DelayedGrab());
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        StartCoroutine(DelayedRelease());
    }

    private IEnumerator DelayedGrab()
    {
        yield return new WaitForSeconds(actionDelay);
        handMesh.enabled = false;
        handArmature.SetActive(false);
    }

    private IEnumerator DelayedRelease()
    {
        yield return new WaitForSeconds(actionDelay);
        handArmature.SetActive(true);
        handMesh.enabled = true;
        emptyHand = true; 
    }

    void Update()
    {
        if (emptyHand && handAnimator != null)
        {
            handAnimator.SetFloat(activateAnim, activateActionRef.action.ReadValue<float>());
            handAnimator.SetFloat(selectAnim, selectActionRef.action.ReadValue<float>());
        }
    }
}
