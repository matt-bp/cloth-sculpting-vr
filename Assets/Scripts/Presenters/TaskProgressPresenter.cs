using System.Collections.Generic;
using System.Linq;
using Models.Local;
using UnityEngine;

namespace Presenters
{
    public class TaskProgressPresenter : MonoBehaviour
    {
        [Header("Model")] [SerializeField] private TaskProgressPositionsModel model;
        
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
                if (i == 0) continue;
                
                Debug.Assert(i >= 0 && i < model.positions.Length);

                var position = model.positions[i - 1];
                
                created.Add(Instantiate(background, position));

                if (i < value.current)
                {
                    created.Add(Instantiate(check, position));
                }
            }
        }
    }
}