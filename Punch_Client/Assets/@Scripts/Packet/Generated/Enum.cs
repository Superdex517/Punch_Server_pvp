// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: Enum.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Google.Protobuf.Protocol {

  /// <summary>Holder for reflection information generated from Enum.proto</summary>
  public static partial class EnumReflection {

    #region Descriptor
    /// <summary>File descriptor for Enum.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static EnumReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "CgpFbnVtLnByb3RvEghQcm90b2NvbCqHAQoMRU9iamVjdFN0YXRlEhYKEkVP",
            "QkpFQ1RfU1RBVEVfTk9ORRAAEhYKEkVPQkpFQ1RfU1RBVEVfSURMRRABEhYK",
            "EkVPQkpFQ1RfU1RBVEVfTU9WRRACEhcKE0VPQkpFQ1RfU1RBVEVfU0tJTEwQ",
            "AxIWChJFT0JKRUNUX1NUQVRFX0RFQUQQBCrPAQoIRU1vdmVEaXISEgoORU1P",
            "VkVfRElSX05PTkUQABIQCgxFTU9WRV9ESVJfVVAQARISCg5FTU9WRV9ESVJf",
            "RE9XThACEhIKDkVNT1ZFX0RJUl9MRUZUEAMSEwoPRU1PVkVfRElSX1JJR0hU",
            "EAQSFQoRRU1PVkVfRElSX1VQX0xFRlQQBRIWChJFTU9WRV9ESVJfVVBfUklH",
            "SFQQBhIXChNFTU9WRV9ESVJfRE9XTl9MRUZUEAcSGAoURU1PVkVfRElSX0RP",
            "V05fUklHSFQQCCo9CgtFR2FtZVVJVHlwZRIWChJFR0FNRV9VSV9UWVBFX05P",
            "TkUQABIWChJFR0FNRV9VSV9UWVBFX1JPT00QASqtAQoPRUdhbWVPYmplY3RU",
            "eXBlEhoKFkVHQU1FX09CSkVDVF9UWVBFX05PTkUQABIaChZFR0FNRV9PQkpF",
            "Q1RfVFlQRV9IRVJPEAESHQoZRUdBTUVfT0JKRUNUX1RZUEVfTU9OU1RFUhAC",
            "EiAKHEVHQU1FX09CSkVDVF9UWVBFX1BST0pFQ1RJTEUQAxIhCh1FR0FNRV9P",
            "QkpFQ1RfVFlQRV9JVEVNX0hPTERFUhAEKjMKDUVHYW1lVGVhbVR5cGUSEAoM",
            "RUdBTUVfVEVBTV9BEAASEAoMRUdBTUVfVEVBTV9CEAEqUgoSRUNlbGxDb2xs",
            "aXNpb25UeXBlEh0KGUVDRUxMX0NPTExJU0lPTl9UWVBFX05PTkUQABIdChlF",
            "Q0VMTF9DT0xMSVNJT05fVFlQRV9XQUxMEAEq4wEKD0VGaW5kUGF0aFJlc3Vs",
            "dBIaChZFRklORF9QQVRIX1JFU1VMVF9OT05FEAASIwofRUZJTkRfUEFUSF9S",
            "RVNVTFRfRkFJTF9MRVJQQ0VMTBABEiIKHkVGSU5EX1BBVEhfUkVTVUxUX0ZB",
            "SUxfTk9fUEFUSBACEiIKHkVGSU5EX1BBVEhfUkVTVUxUX0ZBSUxfTU9WRV9U",
            "TxADEigKJEVGSU5EX1BBVEhfUkVTVUxUX0ZBSUxfU0FNRV9QT1NJVElPThAE",
            "Eh0KGUVGSU5EX1BBVEhfUkVTVUxUX1NVQ0NFU1MQBUIbqgIYR29vZ2xlLlBy",
            "b3RvYnVmLlByb3RvY29sYgZwcm90bzM="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { },
          new pbr::GeneratedClrTypeInfo(new[] {typeof(global::Google.Protobuf.Protocol.EObjectState), typeof(global::Google.Protobuf.Protocol.EMoveDir), typeof(global::Google.Protobuf.Protocol.EGameUIType), typeof(global::Google.Protobuf.Protocol.EGameObjectType), typeof(global::Google.Protobuf.Protocol.EGameTeamType), typeof(global::Google.Protobuf.Protocol.ECellCollisionType), typeof(global::Google.Protobuf.Protocol.EFindPathResult), }, null, null));
    }
    #endregion

  }
  #region Enums
  public enum EObjectState {
    [pbr::OriginalName("EOBJECT_STATE_NONE")] None = 0,
    [pbr::OriginalName("EOBJECT_STATE_IDLE")] Idle = 1,
    [pbr::OriginalName("EOBJECT_STATE_MOVE")] Move = 2,
    [pbr::OriginalName("EOBJECT_STATE_SKILL")] Skill = 3,
    [pbr::OriginalName("EOBJECT_STATE_DEAD")] Dead = 4,
  }

  public enum EMoveDir {
    [pbr::OriginalName("EMOVE_DIR_NONE")] None = 0,
    [pbr::OriginalName("EMOVE_DIR_UP")] Up = 1,
    [pbr::OriginalName("EMOVE_DIR_DOWN")] Down = 2,
    [pbr::OriginalName("EMOVE_DIR_LEFT")] Left = 3,
    [pbr::OriginalName("EMOVE_DIR_RIGHT")] Right = 4,
    [pbr::OriginalName("EMOVE_DIR_UP_LEFT")] UpLeft = 5,
    [pbr::OriginalName("EMOVE_DIR_UP_RIGHT")] UpRight = 6,
    [pbr::OriginalName("EMOVE_DIR_DOWN_LEFT")] DownLeft = 7,
    [pbr::OriginalName("EMOVE_DIR_DOWN_RIGHT")] DownRight = 8,
  }

  public enum EGameUIType {
    [pbr::OriginalName("EGAME_UI_TYPE_NONE")] None = 0,
    [pbr::OriginalName("EGAME_UI_TYPE_ROOM")] Room = 1,
  }

  public enum EGameObjectType {
    [pbr::OriginalName("EGAME_OBJECT_TYPE_NONE")] None = 0,
    [pbr::OriginalName("EGAME_OBJECT_TYPE_HERO")] Hero = 1,
    [pbr::OriginalName("EGAME_OBJECT_TYPE_MONSTER")] Monster = 2,
    [pbr::OriginalName("EGAME_OBJECT_TYPE_PROJECTILE")] Projectile = 3,
    [pbr::OriginalName("EGAME_OBJECT_TYPE_ITEM_HOLDER")] ItemHolder = 4,
  }

  public enum EGameTeamType {
    [pbr::OriginalName("EGAME_TEAM_A")] EgameTeamA = 0,
    [pbr::OriginalName("EGAME_TEAM_B")] EgameTeamB = 1,
  }

  public enum ECellCollisionType {
    [pbr::OriginalName("ECELL_COLLISION_TYPE_NONE")] None = 0,
    [pbr::OriginalName("ECELL_COLLISION_TYPE_WALL")] Wall = 1,
  }

  public enum EFindPathResult {
    [pbr::OriginalName("EFIND_PATH_RESULT_NONE")] None = 0,
    [pbr::OriginalName("EFIND_PATH_RESULT_FAIL_LERPCELL")] FailLerpcell = 1,
    [pbr::OriginalName("EFIND_PATH_RESULT_FAIL_NO_PATH")] FailNoPath = 2,
    [pbr::OriginalName("EFIND_PATH_RESULT_FAIL_MOVE_TO")] FailMoveTo = 3,
    [pbr::OriginalName("EFIND_PATH_RESULT_FAIL_SAME_POSITION")] FailSamePosition = 4,
    [pbr::OriginalName("EFIND_PATH_RESULT_SUCCESS")] Success = 5,
  }

  #endregion

}

#endregion Designer generated code
