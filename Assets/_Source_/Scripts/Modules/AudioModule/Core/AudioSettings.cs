using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using ZLinq;

namespace AudioModule
{
	[CreateAssetMenu(fileName = "AudioData", menuName = "AudioSystem/AudioData")]
	public class AudioSettings : ScriptableObject
	{
		[SerializeField] private AudioData[] _data;

		public Dictionary<AudioID, AudioData> GetData()
		{
			return _data.AsValueEnumerable().ToDictionary(d => d._id, d => d);
		}

		[Serializable]
		public struct AudioData
		{
			[HideLabel]
			[HorizontalGroup]
			public AudioID _id;
			[HideLabel]
			[HorizontalGroup]
			public AudioClip Clip;
			[Range(0f, 1f)]
			public float Volume;
		}
	}
}