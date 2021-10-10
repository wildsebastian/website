{ pkgs ? import <nixpkgs> {} }:

with pkgs;

stdenv.mkDerivation {
  name = "website";

  buildInputs = [
    nodejs
  ];
}
