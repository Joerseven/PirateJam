using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DragandDropManipulator : PointerManipulator
{
    public DragandDropManipulator(VisualElement target)
    {
        this.target = target;
        root = target.parent;
    }

    protected override void RegisterCallbacksOnTarget()
    {
        target.RegisterCallback<PointerDownEvent>(PointerDownHandler);
        target.RegisterCallback<PointerUpEvent>(PointerUpHandler);
        target.RegisterCallback<PointerMoveEvent>(PointerMoveHandler);
        target.RegisterCallback<PointerCaptureOutEvent>(PointerCaptureOutHandler);
    }

    protected override void UnregisterCallbacksFromTarget()
    {
        target.UnregisterCallback<PointerDownEvent>(PointerDownHandler);
        target.UnregisterCallback<PointerUpEvent>(PointerUpHandler);
        target.UnregisterCallback<PointerMoveEvent>(PointerMoveHandler);
        target.UnregisterCallback<PointerCaptureOutEvent>(PointerCaptureOutHandler);
    }

    private Vector2 targetStartPosition { get; set; }

    private Vector3 pointerStartPosition { get; set; }

    private bool enabled { get; set; }

    private VisualElement root { get; }


    //Stores starting position of target and pointer
    //Tells the program a drag is in progress
    private void PointerDownHandler(PointerDownEvent evt)
    {
        targetStartPosition = target.transform.position;
        pointerStartPosition = evt.position;
        targetStartPosition.CapturePointer(evt.pointerId);
        enabled = true;
    }

    //If drag is in progress and target has captured the
    //pointer then calculates new position for target
    private void PointerMoveHandler(PointerMoveEvent evt)
    {
        if (enabled && target.HasPointerCapture(evt.pointerId))
        {
            Vector3 pointerDelta = evt.position - pointerStartPosition;
            target.transform.position = new Vector2(
                Mathf.Clamp(targetStartPosition.x + pointerDelta.x, 0f, target.panel.visualTree.worldBound.width),
                Mathf.Clamp(targetStartPosition.y + pointerDelta.y, 0f, targetStartPosition.panel.visualTree.worldBound.height));
        }
    }

    //Makes target release pointer
    private void PointerUpHandler(PointerUpEvent evt)
    {
        if(enabled && target.HasPointerCapture(evt.pointerId))
        {
            target.ReleasePointer(evt.pointerId);
        }
    }

    //If drag is in progress, queries the tree to find the closest card
    //It then sets the position of the target so it rests on top of the card
    //Otherwise, sets it back to its original pos
    private void PointerCaptureOutHandler(PointerCaptureOutEvent evt)
    {
        if (enabled)
        {
            VisualElement cardContainer = root.Q<VisualElement>("Slots");
            UQueryBuilder<VisualElement> allCards = cardContainer.Query<VisualElement>(className: "Card");
            UQueryBuilder<VisualElement> overlappingCards = allCards.Where(OverlapsTarget);
            VisualElement closestOverlappingCard = FindClosestCard(overlappingCards);
            Vector3 closestPos = Vector3.zero;

            if (closestOverlappingCard != null)
            {
                closestPos = RootSpaceOfCard(closestOverlappingCard);
                closestPos = new Vector2(closestPos.x - 5, closestPos.y - 5);
            }

            target.transform.position = closestOverlappingCard != null ? closestPos : targetStartPosition;

            enabled = false;
        }
    }

    private bool OverlapsTarget(VisualElement card)
    {
        return target.worldBound.Overlaps(card.worldBound);
    }

    private VisualElement FindClosestCard(UQueryBuilder<VisualElement> cards)
    {
        List<VisualElement> cardsList = cards.ToList();
        float bestDistanceSq = float.MaxValue;
        VisualElement closest = null;
        foreach(VisualElement card in cardsList)
        {
            Vector3 displacement RootSpaceOfCard(card) - target.transform.position;
            float distanceSq = displacement.sqrMagnitude;
            if (distanceSq > bestDistanceSq)
            {
                bestDistanceSq = distanceSq;
                closest = card;
            }
        }
        return closest;
    }
    private Vector3 RootSpaceOfCard(VisualElement card)
    {
        Vector2 cardWorldSpace = card.parent.LocalToWorld(card.layout.position);
        return root.WorldToLocal(cardWorldSpace);
    }
}
