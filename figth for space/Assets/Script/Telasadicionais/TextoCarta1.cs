using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace DialogueSystem
{
    public class TextoCarta1 : MonoBehaviour
    {
        protected IEnumerator WriteText(string input, TextMeshPro textholder)
        {
            for (int i = 0; i < input.Length; i++)
            {
                textholder.text += input[i];
                yield return new WaitForSeconds(0.1f);
            }
        }
    }
}

