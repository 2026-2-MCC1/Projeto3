using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class NewMonoBehaviourScript : MonoBehaviour
{

    // esse codigo é para spawnar as notas na tela, ele pega a lista de notas e vai spawnando uma por uma,
    // cada nota tem um xPos diferente, e o yPos é sempre o mesmo, e o zPos é sempre o mesmo, e a rotação é sempre a mesma.

    List<float> whichNote = new List<float>() { 1, 2, 3, 4, 2, 3, 2, 1, 2, 2, 3, 4, 3, 1, 2, 4, 3, 1, 2, 4, 1, 1, 3, 1, 4 };

    public int noteMark = 0;

    public Transform noteObj;

    public string timerResert = "y";

    public float xPos;

    public float yPos;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame

    //o update vai ficar verificando se o timerResert é igual a "y",
    //se for, ele vai chamar a função spawnNote() e mudar o timerResert para "n",
    //assim ele não vai ficar chamando a função spawnNote() toda hora, e quando a função spawnNote() terminar de executar,
    //ela vai mudar o timerResert para "y" novamente, assim o update vai chamar a função spawnNote() de novo.
    void Update()
    {

        if (timerResert == "y")
        {
            StartCoroutine(spawnNote());
            timerResert = "n";
        }

    }

    //essa função vai spawnar a nota na tela, ela vai esperar 1 segundo antes de spawnar a nota, e depois de spawnar a nota,
    IEnumerator spawnNote()
    {
        yield return new WaitForSeconds(1);

        if (whichNote[noteMark] == 1)
        {
            xPos = -0.691f;
        }

        if (whichNote[noteMark] == 2)
        {
            xPos = 0.093f;
        }

        if (whichNote[noteMark] == 3)
        {
            xPos = 0.858f;
        }

        if (whichNote[noteMark] == 4)
        {
            xPos = 1.579f;
        }


        noteMark += 1;
        timerResert = "y";
        Instantiate(noteObj, new Vector3(xPos, 3f, 3.2f), noteObj.rotation);

    }

}
