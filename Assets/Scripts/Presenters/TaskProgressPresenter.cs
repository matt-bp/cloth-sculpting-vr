using System.Collections.Generic;
using UnityEngine;

namespace Presenters
{
    public class TaskProgressPresenter : MonoBehaviour
    {

        [SerializeField] private Transform root;
        [Header("Prefabs")]
        [SerializeField] private GameObject background;
        [SerializeField] private GameObject check;

        private List<GameObject> created = new();

        public void UpdateProgressVisualization((int current, int total) value)
        {
            Debug.Log(value.current);

            foreach (var c in created)
            {
                Destroy(c);
            }

            created = new List<GameObject>();

            float currentX = 0;

            var temp = Instantiate(background, root);
            var updated = temp.transform.position;
            updated.x = currentX;
            temp.transform.position = updated;

            // start here
            var gap = temp.GetComponent<SpriteRenderer>().bounds.size.x;

            currentX += gap;
            
            created.Add(temp);
            

            // Create backgrounds
        }
    }
}