/*Physique : Ragdoll des ennmies morts
 * Les attaques font des explosions qui affecte l'environnement : Boites, porte, etc. et les ennemis touchés : Ragdoll puis animation de se relever
 * Effet sonores : Attaqes, victoire, musique background, etc.
 * Particules : Attaques
 * Joueur ne bouge pas avec le navmesh, mais plutôt avec un blendtree
 * 
 * 
 * 
 * */



//Old code : 



/*
if (Input.GetMouseButtonDown(0) && isRotating == false)
{
	if (moveSelected == 0)
	{
		//Créer un rayon entre la caméra et le curseur.
		Ray camRay = mainCam.ScreenPointToRay(Input.mousePosition);

		RaycastHit hit;
		//Y a-t-il eu un impact
		if (Physics.Raycast(camRay, out hit))
		{
			//Demander au pnj de s'y rendre
			navMeshAgent.SetDestination(hit.point);
			animationJoueur.SetBool("Running", true);
		}
	}
	else if(navMeshAgent.remainingDistance > 0.2f || navMeshAgent.pathPending)
	{
		navMeshAgent.isStopped = true;
		navMeshAgent.ResetPath();
		navMeshAgent.isStopped = false;
	}
	if(navMeshAgent.remainingDistance < 0.2f && navMeshAgent.pathPending == false)
	{
		if (moveSelected == 1 && GameManager.singleton.getTimerJoueur() > 2)
		{
			GameManager.singleton.StartAttack(2);
			Ray camRay = mainCam.ScreenPointToRay(Input.mousePosition);

			RaycastHit hit;
			//Y a-t-il eu un impact
			if (Physics.Raycast(camRay, out hit))
			{


				cible = hit.point;
				rotationTarget = cible;
				rotationTarget.y = transform.position.y;


				isRotating = true;

				//find the vector pointing from our position to the target
				_direction = (rotationTarget - transform.position).normalized;


				//create the rotation we need to be in to look at the target
				_lookRotation = Quaternion.LookRotation(_direction);

				print(_lookRotation.y - transform.rotation.y);
				if (_lookRotation.y - transform.rotation.y > 0)
				{
					animationJoueur.SetTrigger("TurnLeft");

				}
				else if (_lookRotation.y - transform.rotation.y < 0)
				{

					animationJoueur.SetTrigger("TurnRight");
				}


			}


			moveSelected = 0;
		}
	}




}

if (isRotating)
{

	navMeshAgent.isStopped = true;

	//find the vector pointing from our position to the target
	_direction = (rotationTarget - transform.position).normalized;


	//create the rotation we need to be in to look at the target
	_lookRotation = Quaternion.LookRotation(_direction);


	//rotate us over time according to speed until we are in the required rotation
	transform.rotation = Quaternion.RotateTowards(transform.rotation, _lookRotation, Time.deltaTime * 100);


	if (transform.rotation == _lookRotation)
	{
		//particles.transform.position = cible;
		//particles.Play();
		Instantiate(fireball, projectileStartPoint.position, projectileStartPoint.rotation);
		GameManager.singleton.FinishAttack();
		isRotating = false;
		navMeshAgent.isStopped = false;
		animationJoueur.SetBool("TurnRight", false);
	}
}
if (Input.GetMouseButtonDown(1))
{
	if (animationJoueur.GetBool("Running"))
	{
		navMeshAgent.isStopped = true;
		navMeshAgent.ResetPath();
		navMeshAgent.isStopped = false;
		animationJoueur.SetBool("Running", false);
	}
	moveSelected = 0;
}


if (Input.GetKeyDown(KeyCode.Alpha1))
{
	marqueur1.SetActive(true);
	marqueur1.transform.position = transform.position;
	moveSelected = 1;
	if (animationJoueur.GetBool("Running"))
	{
		navMeshAgent.isStopped = true;
		navMeshAgent.ResetPath();
		navMeshAgent.isStopped = false;
		animationJoueur.SetBool("Running", false);
	}
}

if(animationJoueur.GetBool("Running") && navMeshAgent.remainingDistance < 0.2f && navMeshAgent.pathPending == false)
{
	animationJoueur.SetBool("Running", false);
}

if(GameManager.singleton.getTimerJoueur() <= 0.1f)
{
	navMeshAgent.isStopped = true;
	navMeshAgent.ResetPath();
	navMeshAgent.isStopped = false;
	animationJoueur.SetBool("Running", false);
	moveSelected = 0;
}

if(moveSelected == 0)
{
	if(marqueur1.activeSelf == true)
	{
		marqueur1.SetActive(false);
	}
}
if(moveSelected == 1)
{
	Ray camRay = mainCam.ScreenPointToRay(Input.mousePosition);

	RaycastHit hit;
	//Y a-t-il eu un impact
	if (Physics.Raycast(camRay, out hit))
	{
		Vector3 cible = hit.point;
		cible.y = marqueur1.transform.position.y;
		marqueur1.transform.LookAt(cible);
	}
}
*/