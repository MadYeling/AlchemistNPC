using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using ReLogic.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent;
using Terraria.GameContent.Achievements;
using Terraria.GameContent.Events;
using Terraria.GameContent.Tile_Entities;
using Terraria.GameContent.UI;
using Terraria.GameInput;
using Terraria.Graphics.Capture;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.IO;
using Terraria.Localization;
using Terraria.ObjectData;
using Terraria.Social;
using Terraria.UI;
using Terraria.UI.Chat;
using Terraria.UI.Gamepad;
using Terraria.Utilities;
using Terraria.World.Generation;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.GameInput;
using AlchemistNPC;

namespace AlchemistNPC
{
	public class AlchemistNPCPlayer : ModPlayer
	{
		public bool Traps = false;
		public bool Yui = false;
		public bool YuiS = false;
		public bool Extractor = false;
		public bool Scroll = false;
		public bool EyeOfJudgement = false;
		public bool LaetitiaSet = false;
		public bool SF = false;
		public bool PGSWear = false;
		public bool RevSet = false;
		public bool XtraT = false;
		public bool BuffsKeep = false;
		public bool MemersRiposte = false;
		public bool ModPlayer = true;
		public bool lf = false;
		public int lamp = 0;
		public bool ParadiseLost = false;
		public bool Rampage = false;
		public bool LilithEmblem = false;
		public bool trigger = true;
		public bool watchercrystal = false;
		public bool grimreaper = false;
		public bool rainbowdust = false;
		public bool sscope = false;
		public bool lwm = false;
		public bool DB = false;
		
		private const int maxLifeElixir = 2;
		public int LifeElixir = 0;
		private const int maxFuaran = 1;
		public int Fuaran = 0;
		private const int maxKeepBuffs = 1;
		public int KeepBuffs = 0;
		
		public override void ResetEffects()
		{
			AlchemistNPC.BastScroll = false;
			EyeOfJudgement = false;
			LaetitiaSet = false;
			Scroll = false;
			SF = false;
			XtraT = false;
			RevSet = false;
			MemersRiposte = false;
			PGSWear = false;
			Extractor = false;
			ParadiseLost = false;
			Rampage = false;
			LilithEmblem = false;
			watchercrystal = false;
			grimreaper = false;
			rainbowdust = false;
			sscope = false;
			lwm = false;
			Yui = false;
			YuiS = false;
			Traps = false;
			
			player.statLifeMax2 += LifeElixir * 50;
			player.statManaMax2 += Fuaran * 100;
			
			if (KeepBuffs == 1)
			{
			BuffsKeep = true;
			}
			if (KeepBuffs == 0)
			{
			BuffsKeep = false;
			}
		}
	
		public override void clientClone(ModPlayer clientClone)
		{
			AlchemistNPCPlayer clone = clientClone as AlchemistNPCPlayer;
		}
	
		public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
		{
			ModPacket packet = mod.GetPacket();
			packet.Write((byte)AlchemistNPC.AlchemistNPCMessageType.LifeAndManaSync);
			packet.Write((byte)player.whoAmI);
			packet.Write(LifeElixir);
			packet.Write(Fuaran);
			packet.Write(KeepBuffs);
			packet.Send(toWho, fromWho);
		}
	
		public override void OnEnterWorld(Player player)
		{
            string enterText = Language.GetTextValue("Mods.AlchemistNPC.enterText");
			if (ModLoader.GetMod("AlchemistNPCContentDisabler") == null && Config.StartMessage)
			{
            Main.NewText(enterText, 0, 255, 255);
			}
		}
	
		public override void SendClientChanges(ModPlayer clientPlayer)
		{
		}
	
		public override TagCompound Save()
		{
			return new TagCompound {
				{"LifeElixir", LifeElixir},
				{"Fuaran", Fuaran},
				{"KeepBuffs", KeepBuffs},
			};
		}
	
		public override void Load(TagCompound tag)
		{
			LifeElixir = tag.GetInt("LifeElixir");
			Fuaran = tag.GetInt("Fuaran");
			KeepBuffs = tag.GetInt("KeepBuffs");
		}
	
		public override void OnHitNPC(Item item, NPC target, int damage, float knockback, bool crit)
		{	
			if (target.friendly == false)
			{
			if (player.FindBuffIndex(mod.BuffType("RainbowFlaskBuff")) > -1)
				{
				target.buffImmune[BuffID.BetsysCurse] = false;
				target.buffImmune[BuffID.Ichor] = false;
				target.buffImmune[BuffID.Daybreak] = false;
				target.AddBuff(mod.BuffType("Corrosion"), 600);
				target.AddBuff(BuffID.BetsysCurse, 600);
				target.AddBuff(BuffID.Ichor, 600);
				target.AddBuff(BuffID.Daybreak, 600);
				}
			if (player.FindBuffIndex(mod.BuffType("BigBirdLamp")) > -1)
				{
				target.buffImmune[BuffID.BetsysCurse] = false;
				target.buffImmune[BuffID.Ichor] = false;
				target.AddBuff(BuffID.Ichor, 600);
				target.AddBuff(BuffID.BetsysCurse, 600);
				}
			if (Scroll)
				{
					if (target.type != mod.NPCType("Knuckles"))
					{
					target.buffImmune[mod.BuffType("ArmorDestruction")] = false;
					target.AddBuff(mod.BuffType("ArmorDestruction"), 600);
					target.defense = 0;
					}
				}
			if (player.FindBuffIndex(mod.BuffType("ExplorersBrew")) > -1)
				{
				target.AddBuff(mod.BuffType("Electrocute"), 600);
				}
			}
		}
			
		public override void OnHitNPCWithProj(Projectile proj, NPC target, int damage, float knockback, bool crit)
		{
			if (target.friendly == false)
			{
			if (player.FindBuffIndex(mod.BuffType("RainbowFlaskBuff")) > -1)
				{
				target.buffImmune[BuffID.BetsysCurse] = false;
				target.buffImmune[BuffID.Ichor] = false;
				target.buffImmune[BuffID.Daybreak] = false;
				target.AddBuff(mod.BuffType("Corrosion"), 600);
				target.AddBuff(BuffID.BetsysCurse, 600);
				target.AddBuff(BuffID.Ichor, 600);
				target.AddBuff(BuffID.Daybreak, 600);
				}
			if (player.FindBuffIndex(mod.BuffType("BigBirdLamp")) > -1)
				{
				target.buffImmune[BuffID.BetsysCurse] = false;
				target.buffImmune[BuffID.Ichor] = false;
				target.AddBuff(BuffID.Ichor, 600);
				target.AddBuff(BuffID.BetsysCurse, 600);
				}
			if (proj.thrown && Scroll)
				{
				if (target.type != mod.NPCType("Knuckles"))
					{
					target.buffImmune[mod.BuffType("ArmorDestruction")] = false;
					target.AddBuff(mod.BuffType("ArmorDestruction"), 600);
					}
				}
			if ((proj.type == ProjectileID.Electrosphere) && XtraT)
				{
				target.AddBuff(mod.BuffType("Electrocute"), 600);
				}
			if (player.FindBuffIndex(mod.BuffType("ExplorersBrew")) > -1)
				{
				target.AddBuff(mod.BuffType("Electrocute"), 600);
				}
			}
		}
		
		public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
		{
			if (player.FindBuffIndex(mod.BuffType("Uganda")) > -1)
			{
				damageSource = PlayerDeathReason.ByCustomReason(player.name + " DIDN NO DE WEI!");
			}
			if (NPC.AnyNPCs(mod.NPCType("Knuckles")))
			{
				damageSource = PlayerDeathReason.ByCustomReason(player.name + " DIDN NO DE WEI!");
			}
			return true;
		}
		
		public override void ProcessTriggers(TriggersSet triggersSet)
		{
			if (AlchemistNPC.LampLight.JustPressed)
			{
				if (lamp == 0 && trigger)
				{
				trigger = false;
				lamp++;
				lf = true;
				}
				if (lamp == 1 && !trigger && !lf)
				{
				trigger = true;
				lamp = 0;
				}
				lf = false;
			}
			if (AlchemistNPC.DiscordBuff.JustPressed)
			{
				if (Main.myPlayer == player.whoAmI && player.FindBuffIndex(mod.BuffType("DiscordBuff")) > -1)
				{
				Vector2 vector2 = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY);
				player.Teleport(vector2, 1, 0);
				NetMessage.SendData(65, -1, -1, (NetworkText) null, 0, (float) player.whoAmI, (float) vector2.X, (float) vector2.Y, 1, 0, 0);
					if (player.chaosState)
					{
						player.statLife = player.statLife - player.statLifeMax2 / 3;
						PlayerDeathReason damageSource = PlayerDeathReason.ByOther(13);
						if (Main.rand.Next(2) == 0)
						damageSource = PlayerDeathReason.ByOther(player.Male ? 14 : 15);
						if (player.statLife <= 0)
						player.KillMe(damageSource, 1.0, 0, false);
						player.lifeRegenCount = 0;
						player.lifeRegenTime = 0;
					}
				player.AddBuff(88, 600, true);
				player.AddBuff(164, 60, true);
				}
				if (Main.myPlayer == player.whoAmI && player.FindBuffIndex(mod.BuffType("TrueDiscordBuff")) > -1)
				{
				Vector2 vector2 = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY);
				player.Teleport(vector2, 1, 0);
				NetMessage.SendData(65, -1, -1, (NetworkText) null, 0, (float) player.whoAmI, (float) vector2.X, (float) vector2.Y, 1, 0, 0);
					if (player.chaosState)
					{
						player.statLife = player.statLife - player.statLifeMax2 / 7;
						PlayerDeathReason damageSource = PlayerDeathReason.ByOther(13);
						if (Main.rand.Next(2) == 0)
						damageSource = PlayerDeathReason.ByOther(player.Male ? 14 : 15);
						if (player.statLife <= 0)
						player.KillMe(damageSource, 1.0, 0, false);
						player.lifeRegenCount = 0;
						player.lifeRegenTime = 0;
					}
				player.AddBuff(88, 360, true);
				}
			}
		}

		public override void ModifyHitNPC(Item item, NPC target, ref int damage, ref float knockback, ref bool crit)
        {
            if (MemersRiposte && crit)
            {
                damage *= 2;
            }
        }
		
        public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (MemersRiposte && crit)
            {
                damage *= 2;
            }
		}
		
		public override void ModifyHitByNPC(NPC npc, ref int damage, ref bool crit) 		
        {
            if (MemersRiposte)
            {
				for (int h = 0; h < 1; h++) 
					{
					Vector2 vel = new Vector2(0, -1);
					vel *= 0f;
					Projectile.NewProjectile(player.Center.X, player.Center.Y, vel.X, vel.Y, mod.ProjectileType("Returner"), damage*5, 0, player.whoAmI);
					}
			}
			if (player.FindBuffIndex(mod.BuffType("Judgement")) > -1)
				{
					if (Main.rand.Next(3) == 0)
					{
					damage = 2;
					}
				}
			if (ParadiseLost)
				{
				if (damage < 150)
				damage = 1;
				}
        }
		
		public override void ModifyHitByProjectile(Projectile proj, ref int damage, ref bool crit) 	
        {
			if (MemersRiposte)
            {
				for (int h = 0; h < 1; h++) 
					{
					Vector2 vel = new Vector2(0, -1);
					vel *= 0f;
					Projectile.NewProjectile(player.Center.X, player.Center.Y, vel.X, vel.Y, mod.ProjectileType("Returner"), damage*5, 0, player.whoAmI);
					}
			}
			if (player.FindBuffIndex(mod.BuffType("Judgement")) > -1)
				{
					if (Main.rand.Next(3) == 0)
					{
					damage = 2;
					}
				}
			if (ParadiseLost)
				{
				if (damage < 150)
				damage = 1;
				}
        }
	}
}