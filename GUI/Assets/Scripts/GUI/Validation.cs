using System.Collections.Generic;
using UnityEngine;
using Validator;
using Validator.World;

public class Validation
{

	private bool _isDebugMode = false;

	public void DebugModeChanged(bool isDebug)
	{
		_isDebugMode = isDebug;
	}

	public void StartCalculator()
	{
		var manager = GameManager.Instance;
		if (manager == null)
		{
			return;
		}
		List<GUI_TextInputElement> list = manager.GetTextInputField().GetGuiTextElementsWithText();
		List<string> resultSentences = CalculateResultSentences(manager, list);
		List<WorldObject> worldObjs = CalculateWorldObjects();
		CheckValidationResult(worldObjs, resultSentences, list);
	}

	public void StartCalculator(List<GUI_TextInputElement> textputElement, List<string> sentences)
	{
		var manager = GameManager.Instance;
		if (manager == null)
		{
			return;
		}

		List<WorldObject> worldObjs = CalculateWorldObjects();
		CheckValidationResult(worldObjs, sentences, textputElement);
	}

	private List<string> CalculateResultSentences(GameManager manager, List<GUI_TextInputElement> list)
	{
		var resultSentences = new List<string>();
		foreach (GUI_TextInputElement item in list)
		{
			resultSentences.Add(item.GetInputText());
			DebugConsole(item.GetInputText());
		}

		return resultSentences;
	}

	public GameManager GetGameManager()
	{
		return GameManager.Instance;
	}

	private List<WorldObject> CalculateWorldObjects()
	{
		var board = GetGameManager().GetCurrentBoard();

		var obj = board.GetFieldElements();
		List<WorldObject> worldObjs = new List<WorldObject>();
		foreach (Field item in obj)
		{
			if (item.HasPredicateInstance())
			{
				DebugConsole("--- New Element --- ");
				List<Predicate> predicates = item.GetPredicatesList();
				var constant = item.GetConstantsList();
				List<string> worldPredicates = new List<string>();
				foreach (var pred in predicates)
				{
					DebugConsole(pred.PredicateIdentifier);
					worldPredicates.Add(pred.PredicateIdentifier);
				}

				foreach (var co in constant)
				{
					DebugConsole(co);
				}

				List<object> coord = new List<object>();
				coord.Add(item.GetX());
				coord.Add(item.GetZ());
				worldObjs.Add(new WorldObject(constant, worldPredicates, coord));
			}
		}

		return worldObjs;
	}

	private void CheckValidationResult(List<WorldObject> worldObjs, List<string> resultSentences, List<GUI_TextInputElement> list)
	{
		TarskiWorld world = new TarskiWorld();

		WorldResult<EValidationResult> result = world.Check(new WorldParameter(worldObjs, resultSentences));

		for (int i = 0; i < result.Result.Value.Count; i++)
		{
			Result<EValidationResult> item = result.Result.Value[i];
			list[i].Validate(item.Value);

		}

		SetPresentationLayout(world);

	}

	public void SetPresentationLayout()
	{
		TarskiWorld world = new TarskiWorld();
;
		WorldResult<EValidationResult> result = world.Check(new WorldParameter(CalculateWorldObjects(), new List<string>()));
		SetPresentationLayout(world);
	}

	private void SetPresentationLayout(TarskiWorld world)
	{
		var presentationWorldTxt = world.GetPl1Structure().GetModelRepresentation();
		GUI_GetModelPresentation presentation = GameManager.Instance.GetModelPresentation();
		if (presentation != null)
		{
			presentation.SetText(presentationWorldTxt);
		}
	}

	private void DebugConsole(string value)
	{
		if (_isDebugMode)
		{
			Debug.Log(value);
		}
	}
}
