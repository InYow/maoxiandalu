using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;
using UnityEngine.UI;

public class testdialog : MonoBehaviour
{
    public Story story;
    [Header("选项预制件")]
    public DialogChoiceMenu dialogChoiceMenuPrb;
    [Header("当前选项菜单")]
    public DialogChoiceMenu dialogChoiceMenu;
    [Header("UI")]
    public TextMeshProUGUI textGUI;
    public TextMeshProUGUI speaker_NameGUI;
    public Image speaker_Image;
    public GameObject DialogBoxGO;
    [Header("")]
    private TheNode currentNode;
    public TheNode CurrentNode
    {
        get
        {
            return currentNode;
        }
        set
        {
            currentNode = value;
            if (currentNode != null)
            {
                if (currentNode.Speaker != null)
                {
                    speaker_NameGUI.text = currentNode.Speaker;
                }
                if (currentNode.Emotion != null)
                {
                    speaker_Image.sprite = currentNode.Emotion;
                }
                if (currentNode.Text != null)
                {
                    textGUI.text = currentNode.Text;
                }
                currentNode.Readed = true;
            }
            //设置的节点为空
            else
            {
                Debug.Log("设置的节点为空");
            }
        }
    }
    private void Start()
    {
        CurrentNode = story.CurrentNode;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            Next();
        }
    }
    /// <summary>
    /// 在当前节点中进行下一个节点
    /// </summary>
    public void Next()
    {
        TheNode node = story.NextNode;
        if (node != null)
        {
            //Debug.Log("nul Paragraph");
            //节点设置信息
            CurrentNode = node;
        }
        else
        {
            //Debug.Log(node.name);
            //段结束
            Fin();
        }
    }
    /// <summary>
    /// 当前段落结束
    /// </summary>
    private void Fin()
    {
        if (this.dialogChoiceMenu == null)
        {
            List<Choice> choices = story.Choices;
            if (choices != null)
            {
                //打印分支
                //            Debug.Log("打印分支");
                DialogChoiceMenu dialogChoiceMenu = Instantiate(dialogChoiceMenuPrb, transform);
                this.dialogChoiceMenu = dialogChoiceMenu;
                List<DialogChoiceActionInfo> dialogChoiceActionInfos = new();
                foreach (var choice in choices)
                {
                    var capturedChoice = choice;
                    DialogChoiceActionInfo d = new(choice.Text, () => Choose(capturedChoice));
                    dialogChoiceActionInfos.Add(d);
                }
                dialogChoiceMenu.Init(dialogChoiceActionInfos);
            }
            else
            {
                //分支结束了
                Debug.Log("分支结束了");
            }
        }
    }

    /// <summary>
    /// 进入选择的路径
    /// </summary>
    /// <param name="choice"></param>
    public void Choose(Choice choice)
    {
        Paragraph paragraph = story.NextParagraph(choice);
        if (paragraph == null)
        {
            //参数不规范
            Debug.Log("参数不规范 story.NextParagraph(choice);");
        }
        dialogChoiceMenu.Destroy();
        TheNode node = story.CurrentNode;
        if (node == null)
        {
            //空段落，或者说段落结束
            Fin();
            Debug.Log("空段落，或者说段落结束");
        }
        //加载当前段落
        CurrentNode = node;
    }
}
