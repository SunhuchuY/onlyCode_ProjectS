using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class PlayerQuest : MonoBehaviour
{
    public static PlayerQuest instance;

    [SerializeField] private GameObject QuestUI;
    [SerializeField] private GameObject conversationUI;
    [SerializeField] private GameObject ConversationStartObject;
    [SerializeField] private TMP_Text conversationText;
    [SerializeField] private float detectionRadius = 4;
    [SerializeField] private Camera mainCamera;

    [SerializeField] private float zoomInFOV = 30f;
    [SerializeField] private float zoomOutFOV = 60f;
    [SerializeField] private float zoomDuration = 1f;
    [SerializeField] private float followSpeed = 5f;

    public bool isDetectCollider { get; private set; }
    public bool isConversation { get; private set; }

    private Quest quest;
    private Collider DetectionCollider;
    private int conversationCount = 0;

    private void Awake()
    {
        if(instance == null)
            instance = this;    
        else
            Destroy(gameObject);  
    }

    private void LateUpdate()
    {
        DetectQuest();

        // Detect Quest
        if (isDetectCollider)
        {
            SetActiveQuest(true);

            if(conversationCount == 0)
            {
                SetActiveConversationStartObject(true);
            }
            else
            {
                SetActiveConversationStartObject(false);
            }

            if (Input.GetKeyDown(KeyCode.K))
            {
                OnQuesstConversation();
            }

        }
        // Not Detect Quest
        else
        {
            SetActiveQuest(false);
            OffQuesstConversation();
        }


    }

    public void DetectQuest()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius);

        foreach (Collider collider in colliders)
        {
            if (IsQuestCollider(collider))
            {
                isDetectCollider = true;
                DetectionCollider = collider;
                quest = collider.GetComponent<Quest>();
                return;
            }
        }

        isDetectCollider = false;
        DetectionCollider = null;
        quest = null;
    }

    private bool IsQuestCollider(Collider target)
    {
        return target.CompareTag("Quest");
    }

    private void SetActiveQuest(bool active)
    {
        QuestUI.SetActive(active);
    }

    private void SetActiveConversation(bool active)
    {
        conversationUI.SetActive(active);
    }

    private void SetActiveConversationStartObject(bool active)
    {
        ConversationStartObject.SetActive(active);
    }

    public void OnQuesstConversation()
    {
        isConversation = true;
        SetActiveConversation(true);

        if (conversationCount >= quest.Conversation.Length)
        {
            OffQuesstConversation();
        }
        else
        {
            if (conversationCount == 0)
            {
                ZoomIn();
            }

            conversationText.text = quest.Conversation[conversationCount];
            conversationCount++;
        }
    }

    private void OffQuesstConversation()
    {
        isConversation = false;
        SetActiveConversation(false);
        ZoomOut();

        conversationCount = 0;
    }

    private void ZoomIn()
    {
        // 대상의 위치로 카메라를 부드럽게 이동
        Vector3 targetPosition = new Vector3(quest.transform.position.x, quest.transform.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);

        mainCamera.DOFieldOfView(zoomInFOV, zoomDuration);
    }

    private void ZoomOut()
    {
            // Dotween을 사용하여 FOV를 줌아웃으로 변경
        mainCamera.DOFieldOfView(zoomOutFOV, zoomDuration);
    }
}
