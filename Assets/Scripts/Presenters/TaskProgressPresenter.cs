using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Presenters
{
    public class TaskProgressPresenter : MonoBehaviour
    {

        [SerializeField] private Transform[] positions;
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

            foreach (var i in Enumerable.Range(0, value.total))
            {
                Debug.Assert(i >= 0 && i < positions.Length);
                
                created.Add(Instantiate(background, positions[i]));

                if (i < value.current)
                {
                    created.Add(Instantiate(check, positions[i]));
                }
            }
        }
    }
}