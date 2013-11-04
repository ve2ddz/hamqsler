.PHONY: all documentation clean

all: documentation

documentation: validate-docbook pdf-letter pdf-A4

validate-docbook: hamqsler.xml doclicense.xml errorhandling.xml gettingstarted.xml gpl.xml intro.xml macros.xml nkeychars.xml outputtingcards.xml programinit.xml qsovalidation.xml settinguserprefs.xml workingwithcards.xml workingwithqsos.xml
	java -jar bin\jing.jar docbook/docbook.rng hamqsler.xml
	
pdf-letter: hamqsler.xml  doclicense.xml errorhandling.xml gettingstarted.xml gpl.xml intro.xml macros.xml nkeychars.xml outputtingcards.xml programinit.xml qsovalidation.xml settinguserprefs.xml workingwithcards.xml workingwithqsos.xml images/*.png
	bin\xsltproc.exe  \
	    --output hamqsler-letter.fo  \
	    --stringparam body.font.family "Times Roman" \
	    --stringparam fop/fop1.extensions 1  \
	    --stringparam fop/paper.type USLetter \
	    docbook/fo/docbook.xsl  \
	    hamqsler.xml
	fop\fop.cmd -fo hamqsler-letter.fo -pdf pdf-docs/hamqsler-letter.pdf	
	copy NUL pdf-letter

pdf-A4: hamqsler.xml  doclicense.xml errorhandling.xml gettingstarted.xml gpl.xml intro.xml macros.xml nkeychars.xml outputtingcards.xml programinit.xml qsovalidation.xml settinguserprefs.xml workingwithcards.xml workingwithqsos.xml images/*.png
	bin\xsltproc.exe  \
		--output hamqsler-A4.fo  \
   	 --stringparam body.font.family "Times Roman" \
		--stringparam fop1.extensions 1  \
		--stringparam paper.type A4 \
		docbook/fo/docbook.xsl  \
		hamqsler.xml
	fop\fop.cmd -fo hamqsler-A4.fo -pdf pdf-docs/hamqsler-A4.pdf
	copy NUL pdf-A4
	
clean:
	-del pdf-docs\*.pdf *.fo
	-del pdf-letter pdf-A4
