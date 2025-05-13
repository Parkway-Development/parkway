#!/bin/bash

# Load from JSON
export ARM_CLIENT_ID=$(jq -r .clientId terraform-sp.json)
export ARM_CLIENT_SECRET=$(jq -r .clientSecret terraform-sp.json)
export ARM_SUBSCRIPTION_ID=$(jq -r .subscriptionId terraform-sp.json)
export ARM_TENANT_ID=$(jq -r .tenantId terraform-sp.json)

echo "✅ Terraform environment variables exported."
