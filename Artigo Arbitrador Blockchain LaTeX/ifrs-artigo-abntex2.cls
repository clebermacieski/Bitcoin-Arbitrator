%%
%% Customizações do abnTeX2 (http://abnTeX2.googlecode.com) para o Instituto Federal de Educação Ciência e Tecnologia do Rio Grande do Sul - Campus Farroupilha
%% Autor: Cléber Macieski
%% This work may be distributed and/or modified under the
%% conditions of the LaTeX Project Public License, either version 1.3
%% of this license or (at your option) any later version.
%% The latest version of this license is in
%%   http://www.latex-project.org/lppl.txt
%% and version 1.3 or later is part of all distributions of LaTeX
%% version 2005/12/01 or later.
%%
%% This work has the LPPL maintenance status `maintained'.
%% 
%% The Current Maintainer of this work is SEU_NOME, SEU_EMAIL
%%
%% Further information about abnTeX2 are available on http://abntex2.googlecode.com/
%%

\NeedsTeXFormat{LaTeX2e}
\ProvidesClass{ifrs-artigo-abntex2}[2013/04/01 v1.0 Modelo do IFRS baseado em abnTeX2]

%
% Repassa todos os parametros a classe abntex2, 
\DeclareOption*{%
  \PassOptionsToClass{\CurrentOption}{abntex2}%
}
\ProcessOptions\relax
\LoadClass{abntex2}

\renewcommand{\titulo}[1]{\newcommand\@titulo{#1}}
\renewcommand{\imprimirtitulo}{\@titulo}

\newcommand{\curso}[1]{\newcommand\@curso{#1}}
\newcommand{\imprimircurso}{\@curso}

\renewcommand{\autor}[1]{\newcommand\@autor{#1}}
\renewcommand{\imprimirautor}{\@autor}


% alterando a capa
\renewcommand{\imprimircapa}{
  \begin{center}
	  \MakeTextUppercase{\Large\textbf{\imprimirtitulo}}
		\vspace{12pt}
	
    \MakeTextUppercase{\normalsize\imprimircurso}
		\vspace{12pt}

		\textbf\imprimirautor
		
		\textbf\imprimirorientadorRotulo\textbf\imprimirorientador
		
		\imprimirinstituicao
		
		\imprimirlocal
\end{center}
}