provider "scaleway" {
  alias = "tmp"
  region = "fr-par"
}

resource "scaleway_account_project" "website_project" {
  provider = scaleway.tmp
  name = "website"
}

provider "scaleway" {
  project_id = scaleway_account_project.website_project.id
  region = "fr-par"
}

resource "scaleway_registry_namespace" "website_registry" {
  name = "wildsebastian"
  description = "Container registry for wildsebastian.eu"
  is_public = false
}

resource "scaleway_vpc" "website_vpc" {
  name = "wildsebastian"
  tags = ["production"]
}

resource "scaleway_vpc_private_network" "website_private_network" {
  name = "wildsebastian"
  vpc_id = scaleway_vpc.website_vpc.id
  tags = ["production"]

  ipv4_subnet {
    subnet = "10.0.0.0/24"
  }

  ipv6_subnets {
    subnet = "fd00:c2b6:b24b:be67::/64"
  }
}

resource "scaleway_rdb_instance" "website_database" {
  name = "website-db"
  engine    = "PostgreSQL-16"
  node_type = "db-dev-s"
  volume_type = "sbs_15k"
  volume_size_in_gb = "10"
  user_name = "pgadmin"
  password = var.db_admin_password
  is_ha_cluster = false
  tags = ["production"]
  encryption_at_rest = true
  disable_backup = false
  backup_schedule_frequency = 24
  backup_schedule_retention = 7
  backup_same_region = true
  private_network {
    pn_id = scaleway_vpc_private_network.website_private_network.id
    enable_ipam = true
  }
}

resource "scaleway_rdb_acl" "main" {
  instance_id = scaleway_rdb_instance.website_database.id
  acl_rules {
    ip = "62.210.0.0/16"
    description = "Scaleway public ips"
  }

  acl_rules {
    ip = "195.154.0.0/16"
    description = "Scaleway public ips"
  }

  acl_rules {
    ip = "212.129.0.0/18"
    description = "Scaleway public ips"
  }

  acl_rules {
    ip = "62.4.0.0/19"
    description = "Scaleway public ips"
  }

  acl_rules {
    ip = "212.83.128.0/19"
    description = "Scaleway public ips"
  }

  acl_rules {
    ip = "212.83.160.0/19"
    description = "Scaleway public ips"
  }

  acl_rules {
    ip = "212.47.224.0/19"
    description = "Scaleway public ips"
  }

  acl_rules {
    ip = "163.172.0.0/16"
    description = "Scaleway public ips"
  }

  acl_rules {
    ip = "51.15.0.0/16"
    description = "Scaleway public ips"
  }

  acl_rules {
    ip = "151.115.0.0/16"
    description = "Scaleway public ips"
  }

  acl_rules {
    ip = "51.158.0.0/15"
    description = "Scaleway public ips"
  }

  acl_rules {
    ip = "2001:bc8::/32"
    description = "Scaleway public ips"
  }
}

resource "scaleway_rdb_database" "website_database_website" {
  instance_id    = scaleway_rdb_instance.website_database.id
  name           = "website"
}

resource "scaleway_container_namespace" "website_container_namespace" {
  name = "wildsebastian-eu"
  description = "My personal website"
}

resource "scaleway_container" "website_container" {
  name = "wildsebastian"
  description = "My personal website web container"
  namespace_id = scaleway_container_namespace.website_container_namespace.id
  registry_image = "${scaleway_registry_namespace.website_registry.endpoint}/website:latest"
  port = 8080
  cpu_limit = 512
  memory_limit = 1024
  min_scale = 1
  max_scale = 3
  timeout = 600
  privacy = "public"
  protocol = "http1"
  deploy = true

  environment_variables = {
    "ASPNETCORE_ENVIRONMENT" = var.environment
    "ASPNETCORE_GitHub__ClientId" = var.github_client_id
    "ASPNETCORE_FORWARDEDHEADERS_ENABLED" = var.forwarded_headers_enabled
  }

  secret_environment_variables = {
    "ConnectionStrings__DefaultConnection" = "${scaleway_rdb_instance.website_database.private_network[0].hostname};Port=${scaleway_rdb_instance.website_database.private_network[0].port};Username=pgadmin;Password=${var.db_admin_password};Database=${scaleway_rdb_database.website_database_website.name};SslMode=require"
    "ASPNETCORE_GitHub__ClientSecret" = var.github_client_secret
  }

  depends_on = [scaleway_registry_namespace.website_registry, scaleway_container_namespace.website_container_namespace]
}
