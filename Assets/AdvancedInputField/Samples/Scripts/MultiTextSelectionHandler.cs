using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AdvancedInputFieldPlugin.Samples
{
	[RequireComponent(typeof(AdvancedInputField))]
	public class MultiTextSelectionHandler: MonoBehaviour
	{
		[SerializeField]
		private MultiTextSelectionRenderer selectionRenderer;

		private AdvancedInputField inputField;
		private string fullHighlightedText;
		private List<MultiTextSelectionRenderer.TextSelectionRegion> textSelectionRegions;
		private bool backspacePressed;

		private void Awake()
		{
			inputField = GetComponent<AdvancedInputField>();
			textSelectionRegions = new List<MultiTextSelectionRenderer.TextSelectionRegion>();

			selectionRenderer.Initialize(inputField);
			fullHighlightedText = inputField.Text;
		}

		public void OnTextChanged(string text)
		{
			StartCoroutine(DelayedClearHighlight());
		}

		/// <summary>Delayed the highlight clear, because OnSpecialKeyPressed event might be called after OnValueChanged event</summary>
		private IEnumerator DelayedClearHighlight()
        {
			yield return new WaitForSeconds(0.1f);
			if(!backspacePressed) //Only clear text if backspace key wasn't pressed
            {
				ClearHighlight();
            }

			backspacePressed = false;
        }

		public void OnSpecialKeyPressed(SpecialKeyCode keyCode)
		{
			if(keyCode == SpecialKeyCode.BACKSPACE)
			{
				RemoveHighlightedWords();
				backspacePressed = true;
			}
		}

		public void OnTextTap(int tapCount, Vector2 position)
		{
			if(tapCount == 1)
			{
				ClearHighlight();
			}
			else if(tapCount == 3)
			{
				inputField.CaretPosition = inputField.TextSelectionStartPosition;
				HighlightAllWords();
			}
		}

		private void RemoveHighlightedWords()
		{
			if(textSelectionRegions.Count == 0) //Nothing highlighted
			{
				return;
			}

			string text = fullHighlightedText; //Using last known value, because native inputfield might have already deleted a character when pressing backspace key
			StringBuilder stringBuilder = new StringBuilder();
			int selectionRegionIndex = 0;
			MultiTextSelectionRenderer.TextSelectionRegion selectionRegion = textSelectionRegions[selectionRegionIndex];

			int length = text.Length;
			for(int i = 0; i < length; i++)
			{
				char c = text[i];
				if(i >= selectionRegion.startPosition && i < selectionRegion.endPosition)
				{
					continue;
				}
				else
				{
					if(i == selectionRegion.endPosition)
					{
						if(selectionRegionIndex + 1 < textSelectionRegions.Count)
						{
							selectionRegionIndex++;
							selectionRegion = textSelectionRegions[selectionRegionIndex];
						}
					}
					stringBuilder.Append(c);
				}
			}

			string resultText = stringBuilder.ToString();
			inputField.Text = resultText;

			ClearHighlight();
		}

		private void ClearHighlight()
		{
			textSelectionRegions.Clear();
			selectionRenderer.UpdateSelectionRegions(textSelectionRegions);
		}

		private void HighlightAllWords()
		{
			string text = inputField.Text;
			textSelectionRegions.Clear();
			int startPosition = -1;

			int length = text.Length;
			for(int i = 0; i < length; i++)
			{
				char c = text[i];
				if(c == ' ')
				{
					if(startPosition != -1 && startPosition < i)
					{
						textSelectionRegions.Add(new MultiTextSelectionRenderer.TextSelectionRegion(startPosition, i));
						startPosition = -1;
					}
				}
				else if(startPosition == -1)
				{
					startPosition = i;
				}
			}

			if(startPosition != -1 && startPosition < length)
			{
				textSelectionRegions.Add(new MultiTextSelectionRenderer.TextSelectionRegion(startPosition, length));
			}

			selectionRenderer.UpdateSelectionRegions(textSelectionRegions);
			fullHighlightedText = text;
		}
	}
}
