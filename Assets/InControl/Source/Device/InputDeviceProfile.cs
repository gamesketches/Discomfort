﻿using System;
using System.Collections.Generic;
using UnityEngine;


namespace InControl
{
	public abstract class InputDeviceProfile
	{
		[SerializeField]
		public string Name { get; protected set; }

		[SerializeField]
		public string Meta { get; protected set; }

		[SerializeField]
		public InputControlMapping[] AnalogMappings { get; protected set; }

		[SerializeField]
		public InputControlMapping[] ButtonMappings { get; protected set; }

		[SerializeField]
		public string[] IncludePlatforms { get; protected set; }

		[SerializeField]
		public string[] ExcludePlatforms { get; protected set; }

		static HashSet<Type> hideList = new HashSet<Type>();

		float sensitivity = 1.0f;
		float lowerDeadZone = 0.0f;
		float upperDeadZone = 1.0f;


		public InputDeviceProfile()
		{
			Name = "";
			Meta = "";

			AnalogMappings = new InputControlMapping[0];
			ButtonMappings = new InputControlMapping[0];

			IncludePlatforms = new string[0];
			ExcludePlatforms = new string[0];
		}


		[SerializeField]
		public float Sensitivity
		{ 
			get { return sensitivity; }
			protected set { sensitivity = Mathf.Clamp01( value ); }
		}


		[SerializeField]
		public float LowerDeadZone
		{ 
			get { return lowerDeadZone; }
			protected set { lowerDeadZone = Mathf.Clamp01( value ); }
		}


		[SerializeField]
		public float UpperDeadZone
		{ 
			get { return upperDeadZone; }
			protected set { upperDeadZone = Mathf.Clamp01( value ); }
		}


		[Obsolete( "This property has been renamed to IncludePlatforms.", false )]
		public string[] SupportedPlatforms
		{
			get
			{
				return IncludePlatforms;
			}

			protected set
			{
				IncludePlatforms = value;
			}
		}


		public virtual bool IsSupportedOnThisPlatform
		{
			get
			{
				if (ExcludePlatforms != null)
				{
					int excludePlatformsCount = ExcludePlatforms.Length;
					for (int i = 0; i < excludePlatformsCount; i++)
					{
						if (InputManager.Platform.Contains( ExcludePlatforms[i].ToUpper() ))
						{
							return false;
						}
					}
				}

				// TODO: This is weird... what uses this condition? Only UnknownUnityDeviceProfile?
				if (IncludePlatforms == null || IncludePlatforms.Length == 0)
				{
					return true;
				}

				if (IncludePlatforms != null)
				{
					int includePlatformsCount = IncludePlatforms.Length;
					for (int i = 0; i < includePlatformsCount; i++)
					{
						if (InputManager.Platform.Contains( IncludePlatforms[i].ToUpper() ))
						{
							return true;
						}
					}
				}

				return false;
			}
		}


		internal static void Hide( Type type )
		{
			hideList.Add( type );
		}


		internal bool IsHidden
		{
			get { return hideList.Contains( GetType() ); }
		}


		public int AnalogCount
		{
			get { return AnalogMappings.Length; }
		}


		public int ButtonCount
		{
			get { return ButtonMappings.Length; }
		}
	}
}

