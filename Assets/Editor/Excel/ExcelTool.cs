using Excel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

public class ExcelTool : MonoBehaviour
{
    static string excelFilePath = Application.dataPath + "/AssetsPackage/Excel/";
    static string excelCSFilePath = Application.dataPath + "/Scripts/App/EntityData/AutoCreatedRawEntities/";
    static string excelDataFilePath = Application.dataPath + "/StreamingAssets/AutoCreatedRawEntityData/";

    [MenuItem("Tools/Excel/CreateRawEntities")]
    static void ExcelToTable()
    {
        CheckDirectory();

        DirectoryInfo excelParentPathInfo = Directory.GetParent(Application.dataPath);
        string excelPath = excelParentPathInfo.FullName;
        excelPath = excelPath + @"\Excel";

        DirectoryInfo excelPathInfo = new DirectoryInfo(excelFilePath);
        FileInfo[] excels = excelPathInfo.GetFiles();
       
        for (int i = 0; i < excels.Length;i++)
        {
            if (!excels[i].Name.EndsWith(".meta"))
                ReadExcelData(excels[i].FullName);
        }

        Debug.Log("[ExcelTool] Create raw data and raw entites.\r\nDirectory 1: " + excelCSFilePath + "\r\nDirectory 2: " + excelDataFilePath);
        AssetDatabase.Refresh();
    }

    private static void CheckDirectory() 
    {
        if (!Directory.Exists(excelCSFilePath)) 
        {
            Directory.CreateDirectory(excelCSFilePath);
        }
        if (!Directory.Exists(excelDataFilePath))
        {
            Directory.CreateDirectory(excelDataFilePath);
        }
    }

    /// <summary>
    /// 读取Excel表格中的的数据
    /// </summary>
    /// <param name="path"></param>
    static void ReadExcelData(string path)
    {
        List<DataTable> dataTableList = new List<DataTable>();
        using (FileStream stream = File.Open(path, FileMode.Open, FileAccess.Read))
        {
            IExcelDataReader excelReader = null;
            if (path.EndsWith(".xls"))
                excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
            else if (path.EndsWith(".xlsx"))
                excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);

            if (!excelReader.IsValid)
            {
                Debug.LogError("[ExcelTool] Cannot read file at path: " + path);
                return;
            }

            DataSet result = excelReader.AsDataSet();

            //获取数据表文件的每一页数据
            for (int index = 0; index < result.Tables.Count; index++)
            {
                dataTableList.Add(result.Tables[index]);
            }
            excelReader.Close();
            excelReader.Dispose();
        }

        for (int index = 0; index < dataTableList.Count; index++)
        {
            CreateData(path, dataTableList[index]);
        }
    }

    static void CreateData(string path, DataTable dataTable)
    {
        string tableName = PathUtil.GetFileNameWithoutExtension(path) + "_" + dataTable.TableName;

        byte[] buffer = null;
        string[,] dataArr = null;

        using (MemoryStreamInfo ms = new MemoryStreamInfo())
        {
            int row = dataTable.Rows.Count;
            int columns = dataTable.Columns.Count;

            dataArr = new string[columns,3];

            ms.WriteInt(row);
            ms.WriteInt(columns);
            for(int i = 0; i < row;i++)
            {
                for(int j = 0; j < columns;j++)
                {
                    if(i < 3)
                    {
                        dataArr[j, i] = dataTable.Rows[i][j].ToString().Trim();
                    }
                    ms.WriteUTF8String(dataTable.Rows[i][j].ToString().Trim());
                }
            }
            buffer = ms.ToArray();
        }

        //第一步：xor加密
        buffer = SecurityUtil.Xor(buffer);

        //第二步：压缩
        buffer = ZlibHelper.CompressBytes(buffer);

        //第三步：写入文件
        using (FileStream fs = new FileStream(string.Format("{0}{1}", excelDataFilePath, tableName + ".data"), FileMode.Create))
        {
            fs.Write(buffer, 0, buffer.Length);
        }

        CreateEntity(tableName,dataArr);
        CreateEntityModel(tableName, dataArr);
    }

    /// <summary>
    /// 创建实体
    /// </summary>
    private static void CreateEntity(string fileName,string[,] dataArr)
    {
        if(dataArr == null)
        {
            Debug.LogError("读取配表数据有误 ");
            return;
        }

        StringBuilder sbr = new StringBuilder();
        sbr.Append("\r\n");
        sbr.Append("//===================================================\r\n");
        sbr.AppendFormat("//fileName：{0}\r\n", fileName);
        //sbr.AppendFormat("//date：{0}\r\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        sbr.Append("//备    注：此代码为工具生成 请勿手工修改\r\n");
        sbr.Append("//===================================================\r\n");
        sbr.Append("using System.Collections;\r\n");
        sbr.Append("\r\n");
        sbr.Append("/// <summary>\r\n");
        sbr.AppendFormat("/// {0}实体\r\n", fileName);
        sbr.Append("/// </summary>\r\n");
        sbr.AppendFormat("public partial class {0}RawEntity : AbstractRawEntity\r\n", fileName);
        sbr.Append("{\r\n");

        for (int i = 0; i < dataArr.GetLength(0); i++)
        {
            if (i == 0) continue;
            sbr.Append("    /// <summary>\r\n");
            sbr.AppendFormat("    /// {0}\r\n", dataArr[i, 2]);
            sbr.Append("    /// </summary>\r\n");
            sbr.AppendFormat("    public {0} {1} {{ get; set; }}\r\n", dataArr[i, 1], dataArr[i, 0]);
            sbr.Append("\r\n");
        }

        sbr.Append("}\r\n");

        using (FileStream fs = new FileStream(string.Format("{0}{1}RawEntity.cs", excelCSFilePath, fileName), FileMode.Create))
        {
            using (StreamWriter sw = new StreamWriter(fs))
            {
                sw.Write(sbr.ToString());
            }
        }
    }

    /// <summary>
    /// 创建EntityModel
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="dataArr"></param>
    private static void CreateEntityModel(string fileName, string[,] dataArr)
    {
        if (dataArr == null) return;

        StringBuilder sbr = new StringBuilder();
        sbr.Append("\r\n");
        sbr.Append("//===================================================\r\n");
        sbr.AppendFormat("//fileName：{0}\r\n", fileName);
        //sbr.AppendFormat("//date：{0}\r\n", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        sbr.Append("//备    注：此代码为工具生成 请勿手工修改\r\n");
        sbr.Append("//===================================================\r\n");
        sbr.Append("\r\n");
        sbr.Append("using System.Collections;\r\n");
        sbr.Append("using System.Collections.Generic;\r\n");
        sbr.Append("using System;\r\n");
        sbr.Append("\r\n");
        sbr.Append("/// <summary>\r\n");
        sbr.AppendFormat("/// {0}实体数据管理\r\n", fileName);
        sbr.Append("/// </summary>\r\n");
        sbr.AppendFormat("public partial class {0}RawEntityModel : AbstractRawEntityModel<{0}RawEntityModel, {0}RawEntity>\r\n", fileName);
        sbr.Append("{\r\n");
        sbr.Append("    /// <summary>\r\n");
        sbr.Append("    /// 文件名称\r\n");
        sbr.Append("    /// </summary>\r\n");
        sbr.AppendFormat("    protected override string FileName {{ get {{ return \"{0}.data\"; }} }}\r\n", fileName);
        sbr.Append("\r\n");
        sbr.Append("    /// <summary>\r\n");
        sbr.Append("    /// 创建实体\r\n");
        sbr.Append("    /// </summary>\r\n");
        sbr.Append("    /// <param name=\"parse\"></param>\r\n");
        sbr.Append("    /// <returns></returns>\r\n");
        sbr.AppendFormat("    protected override {0}RawEntity MakeEntity(GameEntityParser parse)\r\n", fileName);
        sbr.Append("    {\r\n");
        sbr.AppendFormat("        {0}RawEntity entity = new {0}RawEntity();\r\n", fileName);

        for (int i = 0; i < dataArr.GetLength(0); i++)
        {
            sbr.AppendFormat("        entity.{0} = parse.GetFieldValue(\"{0}\"){1};\r\n", dataArr[i, 0], ChangeTypeName(dataArr[i, 1]));
        }
        sbr.Append("        return entity;\r\n");
        sbr.Append("    }\r\n");
        sbr.Append("}\r\n");

        using (FileStream fs = new FileStream(string.Format("{0}{1}RawEntityModel.cs", excelCSFilePath, fileName), FileMode.Create))
        {
            using (StreamWriter sw = new StreamWriter(fs))
            {
                sw.Write(sbr.ToString());
            }
        }
    }


    private static string ChangeTypeName(string type)
    {
        string str = string.Empty;

        switch (type)
        {
            case "int":
                str = ".ToInt()";
                break;
            case "long":
                str = ".ToLong()";
                break;
            case "float":
                str = ".ToFloat()";
                break;
        }

        return str;
    }



    [MenuItem("Tools/Excel/DeleteRawEntities")]
    static void DeletTable()
    {
        DeleteDir(excelCSFilePath);
        DeleteDir(excelDataFilePath);
        Debug.Log("[ExcelTool] Delete raw data and raw entites.\r\nDirectory 1: " + excelCSFilePath + "\r\nDirectory 2: " + excelDataFilePath);
        AssetDatabase.Refresh();
    }

    static void DeleteDir(string path)
    {
        if (Directory.Exists(path))
        {
            string[] pathArray = Directory.GetFileSystemEntries(path);
            for (int index = 0; index < pathArray.Length; index++)
            {
                if (File.Exists(pathArray[index]))
                {
                    File.Delete(pathArray[index]);
                }
                else
                {
                    DeleteDir(pathArray[index]);
                }
            }
            Directory.Delete(path);
        }
    }
}
