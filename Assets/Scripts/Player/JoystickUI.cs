using UnityEngine;
using UnityEngine.EventSystems;

//누를 때, 드래그할 때, 뗄 때
public class JoystickUI : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [Header("References")]
    [SerializeField] private RectTransform background; //배경 원판
    [SerializeField] private RectTransform handle; //조이스틱

    [Header("Settings")]
    [SerializeField] private float handleRange = 80f; //중심부터의 거리 제한

    public Vector2 InputVector { get; private set; } //조이스틱 입력 방향

    //조이스틱 누르는 순간
    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    //드래그하는 동안
    public void OnDrag(PointerEventData eventData)
    {
        //화면 좌표 -> 로컬 좌표 변환
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            background,
            eventData.position,
            eventData.pressEventCamera,
            out Vector2 localPoint
        ))
        {
            Vector2 radius = background.sizeDelta * 0.5f; //조이스틱 반지름 계산
            Vector2 rawInput = new Vector2(localPoint.x / radius.x, localPoint.y / radius.y);

            InputVector = Vector2.ClampMagnitude(rawInput, 1f); //입력 최대 길이 제한

            handle.anchoredPosition = InputVector * handleRange; //UI
        }
    }

    //떼는 순간
    public void OnPointerUp(PointerEventData eventData)
    {
        InputVector = Vector2.zero; //입력 초기화
        handle.anchoredPosition = Vector2.zero; //UI 원위치
    }
}
