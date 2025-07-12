using UnityEngine;
using UnityEngine.SceneManagement;

namespace UfoPuzzle
{
    public class DragAndDrop : MonoBehaviour
    {
        private Ufo selectedUfo;
        private Vector3 offset;

        private void Update()
        {
            // Handle touch input
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0); // Get the first touch

                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        HandleUfoSelection(touch.position);
                        break;
                    case TouchPhase.Moved:
                        if (selectedUfo)
                        {
                            HandleUfoDragging(touch.position);
                        }
                        break;
                    case TouchPhase.Ended:
                        if (selectedUfo)
                        {
                            GameManager.HandleUfoRelease(selectedUfo);
                            selectedUfo = null;
                        }
                        break;
                }
            }
            
            // Handle mouse input (for testing in the editor)
            else if (Input.GetMouseButtonDown(0))
            {
                HandleUfoSelection(Input.mousePosition);
            }
            else if (Input.GetMouseButton(0) && selectedUfo)
            {
                HandleUfoDragging(Input.mousePosition);
            }
            else if (Input.GetMouseButtonUp(0) && selectedUfo)
            {
                GameManager.HandleUfoRelease(selectedUfo);
                selectedUfo = null;
            }

            if (Input.GetKeyDown(KeyCode.L))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
        
        private void HandleUfoSelection(Vector2 touchPosition)
        {
            Ray ray = Camera.main.ScreenPointToRay(touchPosition);
            if (Physics.Raycast(ray, out RaycastHit hit) /* && GameManager.IsPointerOverUIObject() == false */)
            {
                selectedUfo = hit.transform.GetComponentInParent<Ufo>();
                if (selectedUfo != null)
                {
                    offset = selectedUfo.transform.position - hit.point;
                    // AudioController.Instance.PlayAudio(SoundType.Button);
                    // HapticManager.Play(HapticType.Low_Intensity);
                    selectedUfo.transform.position += new Vector3(0, .7f, 1f);
                    selectedUfo.transform.localScale = Vector3.one;
                }
                else
                {
                    selectedUfo = null;
                }
            }
        }
        
        private void HandleUfoDragging(Vector2 touchPosition)
        {
            GameManager.IsPositionValid(selectedUfo);
            Ray ray = Camera.main.ScreenPointToRay(touchPosition);
            Plane plane = new Plane(Vector3.up, Vector3.up/2.0f);
            
            if (plane.Raycast(ray, out float distance))
            {
                Vector3 targetPosition = ray.GetPoint(distance) + offset;
                selectedUfo.transform.position = targetPosition + new Vector3(0, .7f, 1f);
                if (!selectedUfo.IsSamePosition())
                {
                    // GameManager.HighLightTile(selectedUfo);
                }
            }
        }
    }
}
