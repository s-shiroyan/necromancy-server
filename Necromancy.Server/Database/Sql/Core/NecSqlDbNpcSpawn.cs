using System.Collections.Generic;
using System.Data.Common;
using Necromancy.Server.Model;

namespace Necromancy.Server.Database.Sql.Core
{
    public abstract partial class NecSqlDb<TCon, TCom> : SqlDb<TCon, TCom>
        where TCon : DbConnection
        where TCom : DbCommand
    {
        private const string SqlInsertNpcSpawn =
            "INSERT INTO `nec_npc_spawn` (`id`, `npc_id`, `name`, `status_effect_id`, `status_effect_x`, `status_effect_y`, `status_effect_z`, `to_do`, `special_attribute`, `event_first_encounter`, `event_always`, `play_cutscene_first_encounter`, `play_cutscene_always`, `level`, `title`, `dragon_statue_type`, `icon_type`, `x`, `y`, `z`, `map_id`, `display_condition_flag`, `split_map_number`, `setting_type_flag`, `model_id`, `radius`, `height`, `crouch_height`, `name_plate`, `height_model_attribute`, `z_offset`, `effect_scaling`, `heading`, `created`, `updated`) VALUES(@Id,@npc_id,@name,@status_effect_id,@status_effect_x,@status_effect_y,@status_effect_z,@to_do,@special_attribute,@event_first_encounter,@event_always,@play_cutscene_first_encounter,@play_cutscene_always,@level,@title,@dragon_statue_type,@icon_type,@x,@y,@z,@map_id,@display_condition_flag,@split_map_number,@setting_type_flag,@model_id,@radius,@height,@crouch_height,@name_plate,@height_model_attribute,@z_offset,@effect_scaling,@heading,@created,@updated);";

        private const string SqlSelectNpcSpawns = 
            "SELECT `id`, `npc_id`, `name`, `status_effect_id`, `status_effect_x`, `status_effect_y`, `status_effect_z`, `to_do`, `special_attribute`, `event_first_encounter`, `event_always`, `play_cutscene_first_encounter`, `play_cutscene_always`, `level`, `title`, `dragon_statue_type`, `icon_type`, `x`, `y`, `z`, `map_id`, `display_condition_flag`, `split_map_number`, `setting_type_flag`, `model_id`, `radius`, `height`, `crouch_height`, `name_plate`, `height_model_attribute`, `z_offset`, `effect_scaling`, `heading`, `created`, `updated` FROM `nec_npc_spawn`;";
        
        private const string SqlSelectNpcSpawnsByMapId =
            "SELECT `id`, `npc_id`, `name`, `status_effect_id`, `status_effect_x`, `status_effect_y`, `status_effect_z`, `to_do`, `special_attribute`, `event_first_encounter`, `event_always`, `play_cutscene_first_encounter`, `play_cutscene_always`, `level`, `title`, `dragon_statue_type`, `icon_type`, `x`, `y`, `z`, `map_id`, `display_condition_flag`, `split_map_number`, `setting_type_flag`, `model_id`, `radius`, `height`, `crouch_height`, `name_plate`, `height_model_attribute`, `z_offset`, `effect_scaling`, `heading`, `created`, `updated` FROM `nec_npc_spawn` WHERE `map_id`=@map_id;";

        private const string SqlUpdateNpcSpawn =
            "UPDATE `nec_npc_spawn` SET `npc_id`=@npc_id,`name`=@name,`status_effect_id`=@status_effect_id,`status_effect_x`=@status_effect_x,`status_effect_y`=@status_effect_y,`status_effect_z`=@status_effect_z,`to_do`=@to_do,`special_attribute`=@special_attribute,`event_first_encounter`=@event_first_encounter,`event_always`=@event_always,`play_cutscene_first_encounter`=play_cutscene_first_encounter,`play_cutscene_always`=@play_cutscene_always,`level`=@level,`title`=@title,`dragon_statue_type`=@dragon_statue_type,`icon_type`=@icon_type,`x`=@a,`y`=@y,`z`=@z,`map_id`=@map_id,`display_condition_flag`=@display_condition_flag,`split_map_number`=@split_map_number,`setting_type_flag`=@setting_type_flag,`model_id`=@model_id,`radius`=@radius,`height`=@height,`crouch_height`=@crouch_height,`name_plate`=@name_plate,`height_model_attribute`=@height_model_attribute,`z_offset`=@z_offset,`effect_scaling`=@effect_scaling,`heading`=@heading,`created`=@created,`updated`=@updated FROM `nec_npc_spawn`;";

        private const string SqlDeleteNpcSpawn =
            "DELETE FROM `nec_npc_spawn` WHERE `id`=@id;";

        public bool InsertNpcSpawn(NpcSpawn npcSpawn)
        {
            int rowsAffected = ExecuteNonQuery(SqlInsertNpcSpawn, command =>
            {
                AddParameter(command, "@npc_id", npcSpawn.NpcId);
                AddParameter(command, "@name", npcSpawn.Name);
                AddParameter(command, "@status_effect_id", npcSpawn.StatusEffectId);
                AddParameter(command, "@status_effect_x", npcSpawn.StatusEffectX);
                AddParameter(command, "@status_effect_y", npcSpawn.StatusEffectY);
                AddParameter(command, "@status_effect_z", npcSpawn.StatusEffectZ);
                AddParameter(command, "@to_do", npcSpawn.ToDo);
                AddParameter(command, "@special_attribute", npcSpawn.SpecialAttribute);
                AddParameter(command, "@event_first_encounter", npcSpawn.EventFirstEncounter);
                AddParameter(command, "@event_always", npcSpawn.EventAlways);
                AddParameter(command, "@play_cutscene_first_encounter", npcSpawn.PlayCutsceneFirstEncounter);
                AddParameter(command, "@play_cutscene_always", npcSpawn.PlayCutsceneAlways);
                AddParameter(command, "@level", npcSpawn.Level);
                AddParameter(command, "@title", npcSpawn.Title);
                AddParameter(command, "@dragon_statue_type", npcSpawn.DragonStatueType);
                AddParameter(command, "@icon_type", npcSpawn.IconType);
                AddParameter(command, "@x", npcSpawn.X);
                AddParameter(command, "@y", npcSpawn.Y);
                AddParameter(command, "@z", npcSpawn.Z);
                AddParameter(command, "@map_id", npcSpawn.MapId);
                AddParameter(command, "@display_condition_flag", npcSpawn.DisplayConditionFlag);
                AddParameter(command, "@split_map_number", npcSpawn.SplitMapNumber);
                AddParameter(command, "@setting_type_flag", npcSpawn.SettingTypeFlag);
                AddParameter(command, "@model_id", npcSpawn.ModelId);
                AddParameter(command, "@radius", npcSpawn.Radius);
                AddParameter(command, "@height", npcSpawn.Height);
                AddParameter(command, "@crouch_height", npcSpawn.CrouchHeight);
                AddParameter(command, "@name_plate", npcSpawn.NamePlate);
                AddParameter(command, "@height_model_attribute", npcSpawn.HeightModelAttribute);
                AddParameter(command, "@z_offset", npcSpawn.ZOffset);
                AddParameter(command, "@heading", npcSpawn.Heading);
                AddParameter(command, "@created", npcSpawn.Created);
                AddParameter(command, "@updated", npcSpawn.Updated);
            }, out long autoIncrement);
            if (rowsAffected <= NoRowsAffected || autoIncrement <= NoAutoIncrement)
            {
                return false;
            }

            npcSpawn.Id = (int) autoIncrement;
            return true;
        }

        public List<NpcSpawn> SelectNpcSpawns()
        {
            List<NpcSpawn> npcSpawns = new List<NpcSpawn>();
            ExecuteReader(SqlSelectNpcSpawns, reader =>
            {
                while (reader.Read())
                {
                    NpcSpawn npcSpawn = ReadNpcSpawn(reader);
                    npcSpawns.Add(npcSpawn);
                }
            });
            return npcSpawns;
        }

        public List<NpcSpawn> SelectNpcSpawnsByMapId(int mapId)
        {
            List<NpcSpawn> npcSpawns = new List<NpcSpawn>();
            ExecuteReader(SqlSelectNpcSpawnsByMapId,
                command => { AddParameter(command, "@map_id", mapId); },
                reader =>
                {
                    while (reader.Read())
                    {
                        NpcSpawn npcSpawn = ReadNpcSpawn(reader);
                        npcSpawns.Add(npcSpawn);
                    }
                });
            return npcSpawns;
        }

        public bool UpdateNpcSpawn(NpcSpawn npcSpawn)
        {
            int rowsAffected = ExecuteNonQuery(SqlUpdateNpcSpawn, command =>
            {
                AddParameter(command, "@npc_id", npcSpawn.NpcId);
                AddParameter(command, "@name", npcSpawn.Name);
                AddParameter(command, "@status_effect_id", npcSpawn.StatusEffectId);
                AddParameter(command, "@status_effect_x", npcSpawn.StatusEffectX);
                AddParameter(command, "@status_effect_y", npcSpawn.StatusEffectY);
                AddParameter(command, "@status_effect_z", npcSpawn.StatusEffectZ);
                AddParameter(command, "@to_do", npcSpawn.ToDo);
                AddParameter(command, "@special_attribute", npcSpawn.SpecialAttribute);
                AddParameter(command, "@event_first_encounter", npcSpawn.EventFirstEncounter);
                AddParameter(command, "@event_always", npcSpawn.EventAlways);
                AddParameter(command, "@play_cutscene_first_encounter", npcSpawn.PlayCutsceneFirstEncounter);
                AddParameter(command, "@play_cutscene_always", npcSpawn.PlayCutsceneAlways);
                AddParameter(command, "@level", npcSpawn.Level);
                AddParameter(command, "@title", npcSpawn.Title);
                AddParameter(command, "@dragon_statue_type", npcSpawn.DragonStatueType);
                AddParameter(command, "@icon_type", npcSpawn.IconType);
                AddParameter(command, "@x", npcSpawn.X);
                AddParameter(command, "@y", npcSpawn.Y);
                AddParameter(command, "@z", npcSpawn.Z);
                AddParameter(command, "@map_id", npcSpawn.MapId);
                AddParameter(command, "@display_condition_flag", npcSpawn.DisplayConditionFlag);
                AddParameter(command, "@split_map_number", npcSpawn.SplitMapNumber);
                AddParameter(command, "@setting_type_flag", npcSpawn.SettingTypeFlag);
                AddParameter(command, "@model_id", npcSpawn.ModelId);
                AddParameter(command, "@radius", npcSpawn.Radius);
                AddParameter(command, "@height", npcSpawn.Height);
                AddParameter(command, "@crouch_height", npcSpawn.CrouchHeight);
                AddParameter(command, "@name_plate", npcSpawn.NamePlate);
                AddParameter(command, "@height_model_attribute", npcSpawn.HeightModelAttribute);
                AddParameter(command, "@z_offset", npcSpawn.ZOffset);
                AddParameter(command, "@heading", npcSpawn.Heading);
                AddParameter(command, "@created", npcSpawn.Created);
                AddParameter(command, "@updated", npcSpawn.Updated);
            });
            return rowsAffected > NoRowsAffected;
        }

        public bool DeleteNpcSpawn(int npcSpawnId)
        {
            int rowsAffected = ExecuteNonQuery(SqlDeleteNpcSpawn,
                command => { AddParameter(command, "@id", npcSpawnId); });
            return rowsAffected > NoRowsAffected;
        }

        private NpcSpawn ReadNpcSpawn(DbDataReader reader)
        {
            NpcSpawn npcSpawn = new NpcSpawn();
            npcSpawn.Id = GetInt32(reader, "id");
            npcSpawn.NpcId = GetInt32(reader, "npc_id");
            npcSpawn.Name = GetString(reader, "name");
            npcSpawn.StatusEffectId = GetInt32(reader, "status_effect_id");
            npcSpawn.StatusEffectX = GetFloat(reader, "status_effect_x");
            npcSpawn.StatusEffectY = GetFloat(reader, "status_effect_y");
            npcSpawn.StatusEffectZ = GetFloat(reader, "status_effect_z");
            npcSpawn.ToDo = GetString(reader, "to_do");
            npcSpawn.SpecialAttribute = GetInt32(reader, "special_attribute");
            npcSpawn.EventFirstEncounter = GetInt32(reader, "event_first_encounter");
            npcSpawn.EventAlways = GetInt32(reader, "event_always");
            npcSpawn.PlayCutsceneFirstEncounter = GetInt32(reader, "play_cutscene_first_encounter");
            npcSpawn.PlayCutsceneAlways = GetInt32(reader, "play_cutscene_always");
            npcSpawn.Level = GetByte(reader, "level");
            npcSpawn.Title = GetString(reader, "title");
            npcSpawn.DragonStatueType = GetInt32(reader, "dragon_statue_type");
            npcSpawn.IconType = GetInt32(reader, "icon_type");
            npcSpawn.X = GetFloat(reader, "x");
            npcSpawn.Y = GetFloat(reader, "y");
            npcSpawn.Z = GetFloat(reader, "z");
            npcSpawn.MapId = GetInt32(reader, "map_id");
            npcSpawn.DisplayConditionFlag = GetInt32(reader, "display_condition_flag");
            npcSpawn.SplitMapNumber = GetInt32(reader, "split_map_number");
            npcSpawn.SettingTypeFlag = GetInt32(reader, "setting_type_flag");

            npcSpawn.ModelId = GetInt32(reader, "model_id");

            npcSpawn.Radius = GetInt16(reader, "radius");
            npcSpawn.CrouchHeight = GetByte(reader, "crouch_height");
            npcSpawn.NamePlate = GetByte(reader, "name_plate");
            npcSpawn.HeightModelAttribute = GetInt32(reader, "height_model_attribute");
            npcSpawn.ZOffset = GetInt32(reader, "z_offset");
            npcSpawn.EffectScaling = GetInt32(reader, "effect_scaling");
            npcSpawn.Heading = GetByte(reader, "heading");
            npcSpawn.Created = GetDateTime(reader, "created");
            npcSpawn.Updated = GetDateTime(reader, "updated");

            return npcSpawn;
        }
    }
}
