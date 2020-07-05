using UnityEngine;
using Harmony;

namespace Agony.RadialTabs
{
    internal static class uGUI_CraftNodePatches
    {
		private static int LastIndex = 0;

		[HarmonyPatch(typeof(uGUI_CraftingMenu), nameof(uGUI_CraftingMenu.GetIconMetrics))]
		private static class GetIconMetricsCatcher
        {
			[HarmonyPostfix]
			private static void Postfix(RectTransform canvas, uGUI_CraftingMenu.Node node, int index, int siblings)
            {
				LastIndex = index;
			}
		}

		[HarmonyPatch(typeof(uGUI_CraftingMenu), nameof(uGUI_CraftingMenu.CreateIcon))]
        private static class CreateIconPatch
        {
			[HarmonyPostfix]
            private static void Postfix(uGUI_CraftingMenu.Node node)
            {
                RadialCell radialCell = RadialCell.Create(node, LastIndex);
				var icon = node.icon;
                var vector = new Vector2(radialCell.size, radialCell.size);
				icon.SetBackgroundSize(vector);
				icon.SetActiveSize(vector);
				float num = radialCell.size * (float)Config.IconForegroundSizeMult;
				icon.SetForegroundSize(num, num, true);
				icon.SetBackgroundRadius(radialCell.size / 2f);
				icon.rectTransform.SetParent(node.icon.canvas.transform);
				icon.SetPosition(radialCell.parent.Position);
			}
		}

		[HarmonyPatch(typeof(uGUI_CraftingMenu), nameof(uGUI_CraftingMenu.Expand))]
		private static class SetVisiblePatch
		{
			[HarmonyPostfix]
			private static void Postfix(uGUI_CraftingMenu.Node node)
			{
				if (node.icon == null)
				{
					return;
				}
				RadialCell radialCell = RadialCell.Create(node, LastIndex);
				Vector2 targetPosition = node.expanded ? radialCell.Position : radialCell.parent.Position;
				float speed = (radialCell.radius + radialCell.size) * (float)Config.AnimationSpeedMult;
				float fadeDistance = radialCell.size * (float)Config.AnimationFadeDistanceMult;
				GhostMoving newAnimation = new GhostMoving(speed, fadeDistance, targetPosition);
				ItemIconAnimation.Play(node.icon, newAnimation);
			}
		}

		[HarmonyPatch(typeof(uGUI_CraftingMenu), nameof(uGUI_CraftingMenu.Punch))]
		private static class PunchPatch
		{
			[HarmonyPrefix]
			private static bool Prefix()
			{
				return false;
			}
		}
	}
}
