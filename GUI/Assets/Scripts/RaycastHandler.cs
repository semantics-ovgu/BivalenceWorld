using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class RaycastHandler : MonoBehaviour
    {
        private List<DragObject> _dragObjects = new List<DragObject>();

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _dragObjects = GetRaycastHits();

                foreach (var dragObject in _dragObjects)
                {
                    dragObject.OnMouseDownEvent();
                }
            }
            else if (Input.GetMouseButton(0))
            {
                foreach (var dragObject in _dragObjects)
                {
                    dragObject.OnMouseDragEvent();
                }
            }
            else if (Input.GetMouseButtonUp(0))
            {
                foreach (var dragObject in _dragObjects)
                {
                    dragObject.OnMouseUpEvent();
                }
            }
        }

        private List<DragObject> GetRaycastHits()
        {
            List<DragObject> resultObjects = new List<DragObject>();
            foreach (RaycastHit hit in Physics.RaycastAll(GameManager.Instance.GetScreenToRay()))
            {
                var comp = hit.collider.GetComponent<DragObject>();
                if (comp != null)
                {
                    resultObjects.Add(comp);
                }
            }

            return resultObjects;
        }
    }
}
