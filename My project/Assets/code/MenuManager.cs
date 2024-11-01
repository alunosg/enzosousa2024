using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuManager : MonoBehaviour
{
   [SerializeField] private string NomeLevel;
   [SerializeField] GameObject MenuInicial;
   [SerializeField] GameObject opcoes;
        
   public void jogar()
   {



       SceneManager.LoadScene(NomeLevel);
   }


   public void AbrirOpcoes()
   {
         MenuInicial.SetActive(false);
         opcoes.SetActive(true);
   }


   public void FecharOpcoes()
   {
        opcoes.SetActive(false);
        MenuInicial.SetActive(true);
   }


    public void SairJogo()
   {
       Debug.Log("Sair do jogo");
       Application.Quit();
   }
}
