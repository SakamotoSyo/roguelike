using UnityEngine;
using UnityEngine.UI;

public class DamageUI : MonoBehaviour
{
    [Header("�t�F�[�h�A�E�g����X�s�[�h")]
	[SerializeField] float fadeOutSpeed = 1f;

    [Header("�ړ��l")]
	[SerializeField] float moveSpeed = 0.4f;

    [Tooltip("�e�L�X�g���i�[����ϐ�")]
	Text damageText;
	void Start()
	{
		damageText = GetComponentInChildren<Text>();
	}

	void LateUpdate()
	{
		transform.rotation = Camera.main.transform.rotation;
		transform.position += Vector3.up * moveSpeed * Time.deltaTime;

		damageText.color = Color.Lerp(damageText.color, new Color(1f, 0f, 0f, 0f), fadeOutSpeed * Time.deltaTime);

		if (damageText.color.a <= 0.1f)
		{
			Destroy(gameObject);
		}
	}
}
