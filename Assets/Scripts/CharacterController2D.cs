using UnityEngine;
using UnityEngine.Events;

public class CharacterController2D : MonoBehaviour
{
	[SerializeField] private float m_JumpForce = 400f;							// Amount of force added when the player jumps.
	[Range(0, 1)] [SerializeField] private float m_CrouchSpeed = .36f;          // Amount of maxSpeed applied to crouching movement. 1 = 100%
	[Range(0, 1)] [SerializeField] private float m_SlideSpeed = 1.1f;
	[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;	// How much to smooth out the movement
	[SerializeField] private bool m_AirControl = false;							// Whether or not a player can steer while jumping;
	[SerializeField] private LayerMask m_WhatIsGround;							// A mask determining what is ground to the character
	[SerializeField] private Transform m_GroundCheck;							// A position marking where to check if the player is grounded.
	[SerializeField] private Transform m_CeilingCheck;							// A position marking where to check for ceilings
	[SerializeField] private Collider2D m_CrouchDisableCollider;				// A collider that will be disabled when crouching
	[SerializeField] private Collider2D m_SlideDisableCollider;				

	const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
	private bool m_Grounded;            // Whether or not the player is grounded.
	const float k_CeilingRadius = .2f; // Radius of the overlap circle to determine if the player can stand up
	private Rigidbody2D m_Rigidbody2D;
	public bool m_FacingRight = true;  // For determining which way the player is currently facing.
	private Vector3 m_Velocity = Vector3.zero;

	[Header("Events")]
	[Space]

	public UnityEvent OnLandEvent;

	[System.Serializable]
	public class BoolEvent : UnityEvent<bool> { }

	public BoolEvent OnCrouchEvent;
	public BoolEvent OnSlideEvent;
	private bool m_wasCrouching = false;
	private bool m_wasSliding = false;



	private void Awake()
	{
		m_Rigidbody2D = GetComponent<Rigidbody2D>();

		if (OnLandEvent == null)
			OnLandEvent = new UnityEvent();

		if (OnCrouchEvent == null)
			OnCrouchEvent = new BoolEvent();

		if (OnSlideEvent == null)
			OnSlideEvent = new BoolEvent();
	}

	private void FixedUpdate()
	{
		bool wasGrounded = m_Grounded;
		m_Grounded = false;
	
		//Hr�� je na zemi pokud circlecast groundcheck pozice hitne v�e co je ozna�eno jako "ground"
		Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
			{
				m_Grounded = true;
				if (!wasGrounded)
					OnLandEvent.Invoke();
			}
		}
	}


	public void Move(float move, bool crouch, bool jump, bool sliding)
	{
	
		//Pokud se hr�� kr��, zkontroluj jestli m��e hr�� vst�t
		if (crouch)
		{
			//Pokud m� nad sebou hr�� strop, kter� jim zabra�uje vst�t, nech hr��e kr�et
			if (Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround))
			{
				crouch = true;
			}
		}

		if (sliding)
		{

			//Pokud m� nad sebou hr�� strop, kter� jim zabra�uje vst�t, nech hr��e slidovat
			if (Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround))
			{
				sliding = true;
			}
		}

		//pohybuj hr��e jenom pokud je na zemi nebo pokud je zapnut� airControl:
		if (m_Grounded || m_AirControl)
		{

			// Pokud se kr��
			if (crouch)
			{
				if (!m_wasCrouching)
				{
					m_wasCrouching = true;
					OnCrouchEvent.Invoke(true);
				}

			
				//Sni� rychlost vyn�soben�m rychlost� kr�en�
				move *= m_CrouchSpeed;

				// Vypni jeden z collider� p�i kr�en�
				if (m_CrouchDisableCollider != null)
					m_CrouchDisableCollider.enabled = false;
			} else
			{
			
				// Zapni collider, kdy� se nekr��s
				if (m_CrouchDisableCollider != null)
					m_CrouchDisableCollider.enabled = true;

				if (m_wasCrouching)
				{
					m_wasCrouching = false;
					OnCrouchEvent.Invoke(false);
				}
			}

			//Pokud sliduje�
			if (sliding)
			{
				if (!m_wasSliding)
				{
					m_wasSliding = true;
					OnSlideEvent.Invoke(true);
				}

				// Sni� rychlost vyn�soben�m SlideSpeed
				move *= m_SlideSpeed;

				//Vypni jeden z collider� p�i slidov�n�
				if (m_SlideDisableCollider != null)
					m_SlideDisableCollider.enabled = false;
			}
			else
			{
				//Zapni jeden z collider�, kdy� nesliduje�
				if (m_SlideDisableCollider != null)
					m_SlideDisableCollider.enabled = true;

				if (m_wasSliding)
				{
					m_wasSliding = false;
					OnSlideEvent.Invoke(false);
				}
			}

			
			//H�b�n� hr��em najit�m target velocity
			Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
			// And then smoothing it out and applying it to the character
			//Vyhlazen� a aplikov�n� na hr��e
			m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);



		
			//Pokud input h�be hr��em doprava a hr�� je oto�en� vlevo tak :

			if (move > 0 && !m_FacingRight)
			{
				// oto�en� hr��e
		
				Flip();
				

			}
			//Jinak pokud input h�be hr��em doleva a hr�� je oto�en� doprava :
			else if (move < 0 && m_FacingRight)
			{
				// oto�en� hr��e
				Flip();
			
				
			}
		}
		// Sk�k�n� hr��e
		if (m_Grounded && jump)
		{
			//P�idej vertical force hr��e
			m_Grounded = false;
			m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
		}
	}

	


	private void Flip()
	{
	
		//Zm�� stranu jak se hr�� ot���
		m_FacingRight = !m_FacingRight;

		transform.Rotate(0f, 180f, 0f);
	}



}
