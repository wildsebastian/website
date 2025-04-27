terraform {
  backend "s3" {
    bucket                      = "wildsebastian-terraform-state"
    key                         = "website_production.tfstate"
    region                      = "fr-par"
    endpoint                    = "https://s3.fr-par.scw.cloud"
    skip_credentials_validation = true
    skip_region_validation      = true
    skip_requesting_account_id  = true
  }

  required_providers {
    scaleway = {
      source = "scaleway/scaleway"
    }
  }

  required_version = ">= 1.11.0"
}
