using UnityEngine;
using UnityEngine.UI;

public class DamageUI : MonoBehaviour
{
    [Header("フェードアウトするスピード")]
	[SerializeField] float fadeOutSpeed = 1f;

    [Header("移動値")]
	[SerializeField] float moveSpeed = 0.4f;

    [Tooltip("テキストを格納する変数")]
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
