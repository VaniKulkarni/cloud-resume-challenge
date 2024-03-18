.PHONY : build

build:
	sam build

deploy-infra:
	sam build && aws-vault exec vani1 --no-session -- sam deploy

deploy-site:
	aws-vault exec vani1 --no-session -- aws s3 sync ./frontend s3://first-cloud-resume

deploy-steps:
	sam init
	sam build
	sam deploy 
	aws s3 sync ./frontend s3://vani.kulkarnisworklife.uk
	aws s3 rm s3://vani.kulkarnisworklife.uk --recursive