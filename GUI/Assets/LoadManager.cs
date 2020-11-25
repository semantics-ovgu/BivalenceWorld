using System.Collections;
using System.Collections.Generic;
using System.IO;
using SmartDLL;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadManager : MonoBehaviour
{
	[SerializeField]
	private Button _loadButton = default;
	[SerializeField]
	private TextMeshProUGUI _text = default;

	public SmartFileExplorer fileExplorer = new SmartFileExplorer();

	private bool readText = false;

	void OnEnable()
	{
		_loadButton.onClick.AddListener(delegate { ShowExplorer(); });
	}

	void Update()
	{
		if (fileExplorer.resultOK && readText)
		{
			ReadText(fileExplorer.fileName);
			readText = false;
		}
	}

	void ShowExplorer()
	{
		string initialDir = @"C:\";
		bool restoreDir = true;
		string title = "Open a jsonWld or jsonSen File";
		string defExt = "txt";
		string filter = "txt files (*.jsonSen)|*.jsonSen";

		fileExplorer.OpenExplorer(initialDir, restoreDir, title, defExt, filter);
		readText = true;
	}

	void ReadText(string path)
	{
		_text.text = File.ReadAllText(path);
	}
}
