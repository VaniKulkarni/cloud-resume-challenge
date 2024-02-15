.PHONY : build

build:
	sam build

deploy-infra:
	sam build && aws-vault exec vani1 --no-session -- sam deploy

deploy-site:
	aws-vault exec vani1 --no-session -- aws s3 sync ./src/resume-site s3://first-cloud-resume