using System;
using System.Collections.Generic;
using UnityEngine;

namespace Agony.RadialTabs
{
	public abstract class ItemIconAnimation
	{
		private static void UpdateAnimations()
		{
			List<uGUI_ItemIcon> list = new List<uGUI_ItemIcon>();
			foreach (KeyValuePair<uGUI_ItemIcon, ItemIconAnimation> keyValuePair in animations)
			{
				if (!keyValuePair.Value.OnUpdate(keyValuePair.Key))
				{
					list.Add(keyValuePair.Key);
				}
			}
			list.ForEach(delegate (uGUI_ItemIcon x)
			{
                animations.Remove(x);
			});
		}

		public static void Play(uGUI_ItemIcon actor, ItemIconAnimation newAnimation)
		{
			if (actor == null)
			{
				throw new ArgumentNullException("actor is null");
			}
			if (newAnimation == null)
			{
				throw new ArgumentNullException("newAnimation is null");
			}
            InitializeUpdater();
			if (animations.ContainsKey(actor))
			{
                animations[actor].OnStop(actor);
			}
            animations[actor] = newAnimation;
			newAnimation.OnStart(actor);
		}

		private static void InitializeUpdater()
		{
			if (updater == null)
			{
                updater = new GameObject("ItemIconAnimationUpdater").AddComponent<ItemIconAnimation.Updater>();
			}
		}

		protected abstract void OnStart(uGUI_ItemIcon actor);

		protected abstract bool OnUpdate(uGUI_ItemIcon actor);

		protected abstract void OnStop(uGUI_ItemIcon actor);

		private static Updater updater;

		private static readonly Dictionary<uGUI_ItemIcon, ItemIconAnimation> animations = new Dictionary<uGUI_ItemIcon, ItemIconAnimation>();

		private sealed class Updater : MonoBehaviour
		{
			private void Update()
			{
                UpdateAnimations();
			}
		}
	}
}
