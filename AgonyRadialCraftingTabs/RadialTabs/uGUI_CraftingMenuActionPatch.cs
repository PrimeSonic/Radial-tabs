using Harmony;

namespace Agony.RadialTabs
{
	[HarmonyPatch(typeof(uGUI_CraftingMenu), nameof(uGUI_CraftingMenu.Action))]
	internal static class uGUI_CraftingMenuActionPatch
	{
		[HarmonyPostfix]
		private static void Postfix(uGUI_CraftingMenu __instance, uGUI_CraftingMenu.Node sender)
		{
			bool client = __instance.client != null;
			bool interactable = __instance.interactable;
			if (!client || !interactable || !__instance.ActionAvailable(sender))
			{
				if (sender.icon == null)
				{
					return;
				}
				float duration = 1f + UnityEngine.Random.Range(-0.2f, 0.2f);
				sender.icon.PunchScale(5f, 0.5f, duration, 0f);
			}
		}
	}
}
