using UnityEngine;
using Project.Singleton.ScriptableObjects;
using System.Collections.Generic;
using Project.UI.Cursors;

namespace Project.Singleton.ConfigurationNS
{
	[CreateAssetMenu(fileName ="ConfigurationUI", menuName = "Scriptable Objects/Singleton/Configuration/ConfigurationUI")]
	public class ConfigurationUI : SingletonScriptableObject<ConfigurationUI>
	{
		[SerializeField] List<CustomCursor> _cursors;
		public List<CustomCursor> Cursors => _cursors;
	}
}