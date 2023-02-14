{
  description = "Personal Website";

  inputs = {
    nixpkgs = {
      url = "github:nixos/nixpkgs";
    };

    flake-utils = {
      url = "github:numtide/flake-utils";
    };
  };

  outputs = { self, nixpkgs, flake-utils }:
    flake-utils.lib.eachDefaultSystem (system:
      let pkgs = nixpkgs.legacyPackages.${system}; in
      rec {
        devShell = pkgs.mkShell {
          buildInputs = with pkgs; with nodePackages; [
            nodejs
          ];
        };
      }
    );
}

