using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.Events;

/// <summary>
/// Utilizado para manipular o registro do jogador após o jogo, controlando regras, teclado e poups para usuario.
/// </summary>
namespace Playmove
{
    public class PYScoreManager : MonoBehaviour
    {
        private static PYScoreManager instance;
        public static PYScoreManager Instance
        {
            get
            {
                if (instance == null)
                    instance = (PYScoreManager)FindObjectOfType<PYScoreManager>();

                return instance;
            }
        }

        //private int _score, _difficulty;
        private Action _callback;

        private PYAlertPopup popup;

        void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            PYScoreData.Initialize();
        }

        /// <summary>
        /// Usado para quando clicado na lista
        /// </summary>
        /// <param name="name"></param>
        /// <param name="score"></param>
        /// <param name="difficulty"></param>
        public void UpdateScore(string name, int score, TagManager.GameDifficulty difficulty)
        {
            SetScore("Deseja mesmo registar o resultado com este nome?", name, score, difficulty, PYAudioTags.Voice_ptBR_RegistrarPop02);
        }

        /// <summary>
        /// Usado para entrada no teclado
        /// </summary>
        /// <param name="name"></param>
        /// <param name="score"></param>
        /// <param name="difficulty"></param>
        public void EnterScore(string name, int score, TagManager.GameDifficulty difficulty)
        {
            //SetScore("Este nome já está registrado. Deseja continuar?", name, score, difficulty, PYAudioTags.Voice_Geral_RegistrarPop04);
        }

        void SetScore(string message, string name, int score, TagManager.GameDifficulty difficulty, PYAudioTags voiceMsg)
        {
            if (!char.IsLetter(name[0])) return;

            string[] studentNames = PYScoreData.GetStudentNames();

            PlayTableKeyboard.Instance.SetText(name);
            PlayTableKeyboard.Instance.ShowText();

            if (new List<string>(studentNames).Contains(name))
            {
                string messageText = message;

                PYAudioManager.Instance.StartAudio(voiceMsg).Play();

                PYAudioManager.Instance.StartAudio(PYAudioTags.Voice_ptBR_RegistrarPop01).Play();
                PYAlertPopup popup = PYAlertPopup.InvokeAlertPopup("Popups/AlertPopup_2b").SetTitle("ATENÇÃO!", 1).SetText(messageText.ToUpper(), 1);
                popup.AddButton("PYButtonEffectManager", "NÃO").AddButtonAction(0, () =>
                {
                    PlayTableKeyboard.Instance.Close();
                    PYAudioManager.Instance.StartAudio(PYAudioTags.Voice_ptBR_No).Play();
                });
                popup.AddButton("PYButtonEffectManager_Smaller", "SIM").AddButtonAction(1, () =>
                {
                    CheckScore(name, score, difficulty);
                    PYAudioManager.Instance.StartAudio(PYAudioTags.Voice_ptBR_Yes).Play();
                });
                popup.ClosePopupByFader = false;
                popup.Open();
            }
            else
                RegisterScore(name, score, difficulty);
        }

        void CheckScore(string name, int score, TagManager.GameDifficulty difficulty)
        {
            if (PYScoreData.GetStudentScore(name, difficulty) > score)
            {
                PYAlertPopup popup = PYAlertPopup.InvokeAlertPopup("Popups/AlertPopup_1b").SetTitle("ATENÇÃO!", 1).SetText("O resultado atual é inferior ao já registrado, e por isso será descartado.".ToUpper(), 1);
                popup.AddButton("PYButtonConfirm", null).AddButtonAction(0, () => { BackToMainMenu(); PYAudioManager.Instance.StartAudio(PYAudioTags.Voice_ptBR_Continuar); });
                popup.ClosePopupByFader = false;
                popup.Open();
                PYAudioManager.Instance.StartAudio(PYAudioTags.Voice_ptBR_RegistrarPop03).Delay(.25f).Play();
            }
            else
            {
                RegisterScore(name, score, difficulty);
            }
        }

        void RegisterScore(string name, int score, TagManager.GameDifficulty difficulty)
        {
            PYScoreData.RegisterStudent(name, score, difficulty);
            Invoke("BackToMainMenu", 0.5f);
        }

        void BackToMainMenu()
        {
            PlayTableKeyboard.Instance.Close();
        }
    }
}