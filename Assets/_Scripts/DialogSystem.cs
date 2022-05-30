using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts
{
    public class DialogSystem: StaticInstance<DialogSystem>
    {
        [SerializeField] private CanvasGroup MainDialogCanvas;
        [SerializeField] private CanvasGroup wizardImageCanvasGroup;
        [SerializeField] private TMP_Text _mainTMPText;
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private CanvasGroup ContinueText;
        [SerializeField] private Image characterImage;
        [SerializeField] private PlayerController _playerController;
        [SerializeField] private Animator _animatorWizard;
        [SerializeField] private Animator _animatorEnemy;

        private Queue<string> sentences;
        private bool dialogHasStarted;
        

        private void Start()
        {
            sentences = new Queue<string>();
        }

        private void Update()
        {
            if (Input.GetButtonDown("Submit")&& dialogHasStarted)
            {
                DisplayNextSentence();
                ContinueText.alpha = 0f;
            }
        }

        public void StartDialog(Dialog dialog)
        {
            _playerController.PausePlayer();
            MainDialogCanvas.alpha = 1;
           nameText.text = dialog.name;
           characterImage.sprite =dialog.character;
           
           sentences.Clear();

           foreach (string sentence in dialog.sentences)
           {
               sentences.Enqueue(sentence);
           }

          
           dialogHasStarted = true;
           DisplayNextSentence();
        }

        public void DisplayNextSentence()
        {
            if (sentences.Count == 0)
            {
                dialogHasStarted = false;
                EndDialog();
                return;
            }

            string sentence = sentences.Dequeue();
            StopAllCoroutines();
            StartCoroutine(TypeSentence(sentence));
        }

        private IEnumerator TypeSentence(string sentence)
        {
            _mainTMPText.text = "";
            foreach (char letter in sentence.ToCharArray())
            {
                _mainTMPText.text += letter;
                yield return new WaitForFixedUpdate();
            }

            yield return new WaitForSeconds(1f);
            ContinueText.alpha = 1f;
        }
        private void EndDialog()
        {
           Debug.Log("Endof convo");
           StartCoroutine(UnPausePlayer());
          
           MainDialogCanvas.alpha = 0;
        }

        private IEnumerator UnPausePlayer()
        {
            yield return new WaitForSeconds(1f);
            _playerController.UnpausePlayer();
        }

        public void WizardEnable()
        {
            wizardImageCanvasGroup.alpha = 1;
        }
        public void WizardTalking()
        {
           _animatorWizard.SetBool("Up",true);
           _animatorEnemy.SetBool("Up",false);
           
        }
        public void EnemyTalking()
        {
            _animatorEnemy.SetBool("Up",true);
            _animatorWizard.SetBool("Up", false);
        }
        public void wizardLeft()
        {
            wizardImageCanvasGroup.alpha = 0;
        }
        public void closeDialogBox()
        {
            MainDialogCanvas.alpha = 0;
        }
    }
}