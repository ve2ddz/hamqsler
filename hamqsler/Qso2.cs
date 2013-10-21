/*
 *  Author:
 *       Jim Orcheson <jimorcheson@gmail.com>
 * 
 *  Copyright (c) 2013 Jim Orcheson
 * 
 *  This program is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 * 
 *  This program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 * 
 *  You should have received a copy of the GNU General Public License
 *  along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */
using System;
using System.Collections.Generic;

namespace hamqsler
{
	/// <summary>
	/// Qso2 class - container for AdifFields
	/// </summary>
	public class Qso2
	{
		private AdifEnumerations adifEnums = null;
		private Qsos2 qsos2 = null;
		
		private List<AdifField> fields = new List<AdifField>();
		public List<AdifField> Fields
		{
			get {return fields;}
		}
		
		public int Count
		{
			get {return fields.Count;}
		}
		
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="qsoRecord">string containing the QSO record in ADIF format</param>
		/// <param name="aEnums">AdifEnumerations object</param>
		/// <param name="errorString">string containing error and modification messages</param>
		/// <param name="qsos">Qsos2 object containing the user defined fields</param>
		public Qso2(string qsoRecord, AdifEnumerations aEnums, ref string errorString, Qsos2 qsos=null)
		{
			adifEnums = aEnums;
			qsos2 = qsos;
			AdifFields af = new AdifFields(qsoRecord, ref errorString);
			string[] flds = af.FieldNames;
			string[] types = af.DataTypes;
			string[] values = af.Values;
			List<Userdef> userDefs = null;
			if(qsos != null)
			{
				userDefs = qsos.UserDefs;
			}
			else
			{
				userDefs = new List<Userdef>();
			}
			for(int i = 0; i < af.Count; i++)
			{
				string err = string.Empty;
				string mod = string.Empty;
				switch(flds[i].ToUpper())
				{
					case "A_INDEX":
						A_Index index = new A_Index(values[i]);
						ValidateAndAddField(index, values[i], ref errorString);
						break;
					case "ADDRESS":
						Address addr = new Address(values[i]);
						ValidateAndAddField(addr, values[i], ref errorString);
						break;
					case "AGE":
						Age age = new Age(values[i]);
						ValidateAndAddField(age, values[i], ref errorString);
						break;
					case "ANT_AZ":
						Ant_Az az = new Ant_Az(values[i]);
						ValidateAndAddField(az, values[i], ref errorString);
						break;
					case "ANT_EL":
						Ant_El el = new Ant_El(values[i]);
						ValidateAndAddField(el, values[i], ref errorString);
						break;
					case "ANT_PATH":
						Ant_Path path = new Ant_Path(values[i], aEnums);
						ValidateAndAddField(path, values[i], ref errorString);
						break;
					case "ARRL_SECT":
						Arrl_Sect sect = new Arrl_Sect(values[i], aEnums);
						ValidateAndAddField(sect, values[i], ref errorString);
						break;
					case "AWARD_GRANTED":
						Award_Granted granted = new Award_Granted(values[i], aEnums);
						ValidateAndAddField(granted, values[i], ref errorString);
						break;
					case "AWARD_SUBMITTED":
						Award_Submitted submitted = new Award_Submitted(values[i], aEnums);
						ValidateAndAddField(submitted, values[i], ref errorString);
						break;
					case "BAND_RX":
						Band_Rx bandrx = new Band_Rx(values[i], aEnums);
						ValidateAndAddField(bandrx, values[i], ref errorString);
						break;
					case "BAND":
						Band band = new Band(values[i], aEnums);
						ValidateAndAddField(band, values[i], ref errorString);
						break;
					case "CALL":
						Call call = new Call(values[i]);
						ValidateAndAddField(call, values[i], ref errorString);
						break;
					case "CHECK":
						Check check = new Check(values[i]);
						ValidateAndAddField(check, values[i], ref errorString);
						break;
					case "CLASS":
						Class cls = new Class(values[i]);
						ValidateAndAddField(cls, values[i], ref errorString);
						break;
					case "CLUBLOG_QSO_UPLOAD_DATE":
						Clublog_Qso_Upload_Date cdate = new Clublog_Qso_Upload_Date(values[i]);
						ValidateAndAddField(cdate, values[i], ref errorString);
						break;
					case "CLUBLOG_QSO_UPLOAD_STATUS":
						Clublog_Qso_Upload_Status cstatus = new Clublog_Qso_Upload_Status(values[i],
						                                                                  aEnums);
						ValidateAndAddField(cstatus, values[i], ref errorString);
						break;
					case "CNTY":
						Cnty cnty = new Cnty(values[i]);
						ValidateAndAddField(cnty, values[i], ref errorString);
						break;
					case "COMMENT":
						Comment comment = new Comment(values[i]);
						ValidateAndAddField(comment, values[i], ref errorString);
						break;
					case "CONT":
						Cont cont = new Cont(values[i], aEnums);
						ValidateAndAddField(cont, values[i], ref errorString);
						break;
					case "CONTACTED_OP":
						Contacted_Op cop = new Contacted_Op(values[i]);
						ValidateAndAddField(cop, values[i], ref errorString);
						break;
					case "CONTEST_ID":
						Contest_Id contest = new Contest_Id(values[i], aEnums);
						ValidateAndAddField(contest, values[i], ref errorString);
						break;
					case "COUNTRY":
						Country country = new Country(values[i], aEnums);
						ValidateAndAddField(country, values[i], ref errorString);
						break;
					case "CQZ":
						CQZ cqz = new CQZ(values[i]);
						ValidateAndAddField(cqz, values[i], ref errorString);
						break;
					case "CREDIT_GRANTED":
						Credit_Granted credGranted = new Credit_Granted(values[i], aEnums);
						ValidateAndAddField(credGranted, values[i], ref errorString);
						break;
					case "CREDIT_SUBMITTED":
						Credit_Submitted credSubmitted = new Credit_Submitted(values[i], aEnums);
						ValidateAndAddField(credSubmitted, values[i], ref errorString);
						break;
					case "DISTANCE":
						Distance dist = new Distance(values[i]);
						ValidateAndAddField(dist, values[i], ref errorString);
						break;
					case "DXCC":
						DXCC dxcc = new DXCC(values[i], aEnums);
						ValidateAndAddField(dxcc, values[i], ref errorString);
						break;
					case "EMAIL":
						Email email = new Email(values[i]);
						ValidateAndAddField(email, values[i], ref errorString);
						break;
					case "EQ_CALL":
						Eq_Call eqcall = new Eq_Call(values[i]);
						ValidateAndAddField(eqcall, values[i], ref errorString);
						break;
					case "EQSL_QSLRDATE":
						Eqsl_QslRDate erdate = new Eqsl_QslRDate(values[i]);
						ValidateAndAddField(erdate, values[i], ref errorString);
						break;
					case "EQSL_QSLSDATE":
						Eqsl_QslSDate esdate = new Eqsl_QslSDate(values[i]);
						ValidateAndAddField(esdate, values[i], ref errorString);
						break;
					case "EQSL_QSL_RCVD":
						Eqsl_Qsl_Rcvd eqrcvd = new Eqsl_Qsl_Rcvd(values[i], aEnums);
						ValidateAndAddField(eqrcvd, values[i], ref errorString);
						break;
					case "EQSL_QSL_SENT":
						Eqsl_Qsl_Sent eqsent = new Eqsl_Qsl_Sent(values[i], aEnums);
						ValidateAndAddField(eqsent, values[i], ref errorString);
						break;
					case "FISTS":
						Fists fists = new Fists(values[i]);
						ValidateAndAddField(fists, values[i], ref errorString);
						break;
					case "FISTS_CC":
						Fists_CC fistscc = new Fists_CC(values[i]);
						ValidateAndAddField(fistscc, values[i], ref errorString);
						break;
					case "FORCE_INIT":
						Force_Init finit = new Force_Init(values[i]);
						ValidateAndAddField(finit, values[i], ref errorString);
						break;
					case "FREQ":
						Freq freq = new Freq(values[i], aEnums);
						ValidateAndAddField(freq, values[i], ref errorString);
						break;
					case "FREQ_RX":
						Freq_Rx freqrx = new Freq_Rx(values[i], aEnums);
						ValidateAndAddField(freqrx, values[i], ref errorString);
						break;
					case "GRIDSQUARE":
						GridSquare grid = new GridSquare(values[i]);
						ValidateAndAddField(grid, values[i], ref errorString);
						break;
					case "GUEST_OP":
						Guest_Op guest = new Guest_Op(values[i]);
						ValidateAndAddField(guest, values[i], ref errorString);
						break;
					case "HRDLOG_QSO_UPLOAD_DATE":
						HrdLog_Qso_Upload_Date hdate = new HrdLog_Qso_Upload_Date(values[i]);
						ValidateAndAddField(hdate, values[i], ref errorString);
						break;
					case "HRDLOG_QSO_UPLOAD_STATUS":
						HrdLog_Qso_Upload_Status hstatus = new HrdLog_Qso_Upload_Status(values[i], aEnums);
						ValidateAndAddField(hstatus, values[i], ref errorString);
						break;
					case "IOTA":
						Iota iota = new Iota(values[i], aEnums);
						ValidateAndAddField(iota, values[i], ref errorString);
						break;
					case "IOTA_ISLAND_ID":
						Iota_Island_ID iid = new Iota_Island_ID(values[i]);
						ValidateAndAddField(iid, values[i], ref errorString);
						break;
					case "ITUZ":
						ITUZ ituz = new ITUZ(values[i]);
						ValidateAndAddField(ituz, values[i], ref errorString);
						break;
					case "K_INDEX":
						K_Index kindex = new K_Index(values[i]);
						ValidateAndAddField(kindex, values[i], ref errorString);
						break;
					case "LAT":
						Lat lat = new Lat(values[i]);
						ValidateAndAddField(lat, values[i], ref errorString);
						break;
					case "LON":
						Lon lon = new Lon(values[i]);
						ValidateAndAddField(lon, values[i], ref errorString);
						break;
					case "LOTW_QSLRDATE":
						Lotw_QslRDate lrdate = new Lotw_QslRDate(values[i]);
						ValidateAndAddField(lrdate, values[i], ref errorString);
						break;
					case "LOTW_QSLSDATE":
						Lotw_QslSDate lsdate = new Lotw_QslSDate(values[i]);
						ValidateAndAddField(lsdate, values[i], ref errorString);
						break;
					case "LOTW_QSL_RCVD":
						Lotw_Qsl_Rcvd lrcvd = new Lotw_Qsl_Rcvd(values[i], aEnums);
						ValidateAndAddField(lrcvd, values[i], ref errorString);
						break;
					case "LOTW_QSL_SENT":
						Lotw_Qsl_Sent lsent = new Lotw_Qsl_Sent(values[i], aEnums);
						ValidateAndAddField(lsent, values[i], ref errorString);
						break;
					case "MAX_BURSTS":
						Max_Bursts bursts = new Max_Bursts(values[i]);
						ValidateAndAddField(bursts, values[i], ref errorString);
						break;
					case "MODE":
						Mode mode = new Mode(values[i], aEnums);
						ValidateAndAddField(mode, values[i], ref errorString);
						break;
					case "MS_SHOWER":
						Ms_Shower shower = new Ms_Shower(values[i]);
						ValidateAndAddField(shower, values[i], ref errorString);
						break;
					case "MY_CITY":
						My_City mycity = new My_City(values[i]);
						ValidateAndAddField(mycity, values[i], ref errorString);
						break;
					case "MY_CNTY":
						My_Cnty mycnty = new My_Cnty(values[i]);
						ValidateAndAddField(mycnty, values[i], ref errorString);
						break;
					case "MY_COUNTRY":
						My_Country mycountry = new My_Country(values[i], aEnums);
						ValidateAndAddField(mycountry, values[i], ref errorString);
						break;
					case "MY_CQ_ZONE":
						My_CQ_Zone mycqzone = new My_CQ_Zone(values[i]);
						ValidateAndAddField(mycqzone, values[i], ref errorString);
						break;
					case "MY_DXCC":
						My_DXCC mydxcc = new My_DXCC(values[i], aEnums);
						ValidateAndAddField(mydxcc, values[i], ref errorString);
						break;
					case "MY_FISTS":
						My_Fists myfists = new My_Fists(values[i]);
						ValidateAndAddField(myfists, values[i], ref errorString);
						break;
					case "MY_GRIDSQUARE":
						My_GridSquare mygrid = new My_GridSquare(values[i]);
						ValidateAndAddField(mygrid, values[i], ref errorString);
						break;
					case "MY_IOTA":
						My_Iota myiota = new My_Iota(values[i], aEnums);
						ValidateAndAddField(myiota, values[i], ref errorString);
						break;
					case "MY_IOTA_ISLAND_ID":
						My_Iota_Island_ID myiid = new My_Iota_Island_ID(values[i]);
						ValidateAndAddField(myiid, values[i], ref errorString);
						break;
					case "MY_ITU_ZONE":
						My_ITU_Zone myizone = new My_ITU_Zone(values[i]);
						ValidateAndAddField(myizone, values[i], ref errorString);
						break;
					case "MY_LAT":
						My_Lat mylat = new My_Lat(values[i]);
						ValidateAndAddField(mylat, values[i], ref errorString);
						break;
					case "MY_LON":
						My_Lon mylon = new My_Lon(values[i]);
						ValidateAndAddField(mylon, values[i], ref errorString);
						break;
					case "MY_NAME":
						My_Name myname = new My_Name(values[i]);
						ValidateAndAddField(myname, values[i], ref errorString);
						break;
					case "MY_POSTAL_CODE":
						My_Postal_Code mycode = new My_Postal_Code(values[i]);
						ValidateAndAddField(mycode, values[i], ref errorString);
						break;
					case "MY_RIG":
						My_Rig myrig = new My_Rig(values[i]);
						ValidateAndAddField(myrig, values[i], ref errorString);
						break;
					case "MY_SIG":
						My_Sig mysig = new My_Sig(values[i]);
						ValidateAndAddField(mysig, values[1], ref errorString);
						break;
					case "MY_SIG_INFO":
						My_Sig_Info myinfo = new My_Sig_Info(values[i]);
						ValidateAndAddField(myinfo, values[i], ref errorString);
						break;
					case "MY_SOTA_REF":
						My_Sota_Ref mysota = new My_Sota_Ref(values[i]);
						ValidateAndAddField(mysota, values[i], ref errorString);
						break;
					case "MY_STATE":
						My_State mystate = new My_State(values[i]);
						ValidateAndAddField(mystate, values[i], ref errorString);
						break;
					case "MY_STREET":
						My_Street mystreet = new My_Street(values[i]);
						ValidateAndAddField(mystreet, values[i], ref errorString);
						break;
					case "MY_USACA_COUNTIES":
						My_Usaca_Counties myusa = new My_Usaca_Counties(values[i]);
						ValidateAndAddField(myusa, values[i], ref errorString);
						break;
					case "MY_VUCC_GRIDS":
						My_VUCC_Grids mygrids = new My_VUCC_Grids(values[i]);
						ValidateAndAddField(mygrids, values[i], ref errorString);
						break;
					case "NAME":
						Name name = new Name(values[i]);
						ValidateAndAddField(name, values[i], ref errorString);
						break;
					case "NOTES":
						Notes notes = new Notes(values[i]);
						ValidateAndAddField(notes, values[i], ref errorString);
						break;
					case "NR_BURSTS":
						Nr_Bursts numBursts = new Nr_Bursts(values[i]);
						ValidateAndAddField(numBursts, values[i], ref errorString);
						break;
					case "NR_PINGS":
						Nr_Pings pings = new Nr_Pings(values[i]);
						ValidateAndAddField(pings, values[i], ref errorString);
						break;
					case "OPERATOR":
						Operator oper = new Operator(values[i]);
						ValidateAndAddField(oper, values[i], ref errorString);
						break;
					case "OWNER_CALLSIGN":
						Owner_Callsign ownerCall = new Owner_Callsign(values[i]);
						ValidateAndAddField(ownerCall, values[i], ref errorString);
						break;
					case "PFX":
						Pfx pfx = new Pfx(values[i]);
						ValidateAndAddField(pfx, values[i], ref errorString);
						break;
					case "PRECEDENCE":
						Precedence prec = new Precedence(values[i]);
						ValidateAndAddField(prec, values[i], ref errorString);
						break;
					case "PROP_MODE":
						Prop_Mode pMode = new Prop_Mode(values[i], aEnums);
						ValidateAndAddField(pMode, values[i], ref errorString);
						break;
					case "PUBLIC_KEY":
						Public_Key key = new Public_Key(values[i]);
						ValidateAndAddField(key, values[i], ref errorString);
						break;
					case "QRZCOM_QSO_UPLOAD_DATE":
						QrzCom_Qso_Upload_Date qdate = new QrzCom_Qso_Upload_Date(values[i]);
						ValidateAndAddField(qdate, values[i], ref errorString);
						break;
					case "QRZCOM_QSO_UPLOAD_STATUS":
						QrzCom_Qso_Upload_Status qstatus = new QrzCom_Qso_Upload_Status(values[i], aEnums);
						ValidateAndAddField(qstatus, values[i], ref errorString);
						break;
					case "QSLMSG":
						QslMsg qMsg = new QslMsg(values[i]);
						ValidateAndAddField(qMsg, values[i], ref errorString);
						break;
					case "QSLRDATE":
						QslRDate qrdate = new QslRDate(values[i]);
						ValidateAndAddField(qrdate, values[i], ref errorString);
						break;
					case "QSLSDATE":
						QslSDate qsdate = new QslSDate(values[i]);
						ValidateAndAddField(qsdate, values[i], ref errorString);
						break;
					case "QSL_RCVD":
						Qsl_Rcvd rcvd = new Qsl_Rcvd(values[i], aEnums);
						ValidateAndAddField(rcvd, values[i], ref errorString);
						break;
					case "QSL_RCVD_VIA":
						Qsl_Rcvd_Via rvia = new Qsl_Rcvd_Via(values[i], aEnums);
						ValidateAndAddField(rvia, values[i], ref errorString);
						break;
					case "QSL_SENT":
						Qsl_Sent sent = new Qsl_Sent(values[i], aEnums);
						ValidateAndAddField(sent, values[i], ref errorString);
						break;
					case "QSL_SENT_VIA":
						Qsl_Sent_Via svia = new Qsl_Sent_Via(values[i], aEnums);
						ValidateAndAddField(svia, values[i], ref errorString);
						break;
					case "QSL_VIA":
						Qsl_Via qvia = new Qsl_Via(values[i]);
						ValidateAndAddField(qvia, values[i], ref errorString);
						break;
					case "QSO_COMPLETE":
						Qso_Complete complete = new Qso_Complete(values[i], aEnums);
						ValidateAndAddField(complete, values[i], ref errorString);
						break;
					case "QSO_DATE":
						Qso_Date date = new Qso_Date(values[i]);
						ValidateAndAddField(date, values[i], ref errorString);
						break;
					case "QSO_DATE_OFF":
						Qso_Date_Off dateoff = new Qso_Date_Off(values[i]);
						ValidateAndAddField(dateoff, values[i], ref errorString);
						break;
					case "QSO_RANDOM":
						Qso_Random random = new Qso_Random(values[i]);
						ValidateAndAddField(random, values[i], ref errorString);
						break;
					case "QTH":
						Qth qth = new Qth(values[i]);
						ValidateAndAddField(qth, values[i], ref errorString);
						break;
					case "RIG":
						Rig rig = new Rig(values[i]);
						ValidateAndAddField(rig, values[i], ref errorString);
						break;
					case "RST_RCVD":
						Rst_Rcvd rstr = new Rst_Rcvd(values[i]);
						ValidateAndAddField(rstr, values[i], ref errorString);
						break;
					case "RST_SENT":
						Rst_Sent rsts = new Rst_Sent(values[i]);
						ValidateAndAddField(rsts, values[i], ref errorString);
						break;
					case "RX_PWR":
						Rx_Pwr rxpwr = new Rx_Pwr(values[i]);
						ValidateAndAddField(rxpwr, values[i], ref errorString);
						break;
					case "SAT_MODE":
						Sat_Mode smode = new Sat_Mode(values[i]);
						ValidateAndAddField(smode, values[i], ref errorString);
						break;
					case "SAT_NAME":
						Sat_Name sname = new Sat_Name(values[i]);
						ValidateAndAddField(sname, values[i], ref errorString);
						break;
					case "SFI":
						SFI sfi = new SFI(values[i]);
						ValidateAndAddField(sfi, values[i], ref errorString);
						break;
					case "SIG":
						Sig sig = new Sig(values[i]);
						ValidateAndAddField(sig, values[i], ref errorString);
						break;
					case "SIG_INFO":
						Sig_Info sinfo = new Sig_Info(values[i]);
						ValidateAndAddField(sinfo, values[i], ref errorString);
						break;
					case "SKCC":
						SKCC skcc = new SKCC(values[i]);
						ValidateAndAddField(skcc, values[i], ref errorString);
						break;
					case "SOTA_REF":
						Sota_Ref sref = new Sota_Ref(values[i]);
						ValidateAndAddField(sref, values[i], ref errorString);
						break;
					case "SRX":
						Srx srx = new Srx(values[i]);
						ValidateAndAddField(srx, values[i], ref errorString);
						break;
					case "SRX_STRING":
						Srx_String ssrx = new Srx_String(values[i]);
						ValidateAndAddField(ssrx, values[i], ref errorString);
						break;
					case "STATE":
						State state = new State(values[i]);
						ValidateAndAddField(state, values[i], ref errorString);
						break;
					case "STATION_CALLSIGN":
						Station_Callsign scall = new Station_Callsign(values[i]);
						ValidateAndAddField(scall, values[i], ref errorString);
						break;
					case "STX":
						Stx stx = new Stx(values[i]);
						ValidateAndAddField(stx, values[i], ref errorString);
						break;
					case "STX_STRING":
						Stx_String sstx = new Stx_String(values[i]);
						ValidateAndAddField(sstx, values[i], ref errorString);
						break;
					case "SUBMODE":
						Submode submode = new Submode(values[i], aEnums);
						ValidateAndAddField(submode, values[i], ref errorString);
						break;
					case "SWL":
						SWL swl = new SWL(values[i]);
						ValidateAndAddField(swl, values[i], ref errorString);
						break;
					case "TEN_TEN":
						Ten_Ten tt = new Ten_Ten(values[i]);
						ValidateAndAddField(tt, values[i], ref errorString);
						break;
					case "TIME_OFF":
						Time_Off to = new Time_Off(values[i]);
						ValidateAndAddField(to, values[i], ref errorString);
						break;
					case "TIME_ON":
						Time_On time = new Time_On(values[i]);
						ValidateAndAddField(time, values[i], ref errorString);
						break;
					case "TX_PWR":
						Tx_Pwr txpwr = new Tx_Pwr(values[i]);
						ValidateAndAddField(txpwr, values[i], ref errorString);
						break;
					case "USACA_COUNTIES":
						Usaca_Counties usaca = new Usaca_Counties(values[i]);
						ValidateAndAddField(usaca, values[i], ref errorString);
						break;
					case "VE_PROV":
						VE_Prov prov = new VE_Prov(values[i]);
						ValidateAndAddField(prov, values[i], ref errorString);
						break;
					case "VUCC_GRIDS":
						VUCC_Grids grids = new VUCC_Grids(values[i]);
						ValidateAndAddField(grids, values[i], ref errorString);
						break;
					case "WEB":
						Web web = new Web(values[i]);
						ValidateAndAddField(web, values[i], ref errorString);
						break;
					default:
						if(flds[i].ToUpper().StartsWith("APP_"))
						{
							ApplicationDefinedField adf = new ApplicationDefinedField(flds[i],
							                                                          types[i],
							                                                          values[i],
							                                                          aEnums);
							ValidateAndAddField(adf, values[i], ref errorString);
						}
						else
						{
							bool userDefFound = false;
							foreach(Userdef uDef in userDefs)
							{
								if(flds[i].Equals(uDef.UName))
								{
									switch(uDef.DataType.Value)
									{
										case "B":
											UserdefBoolean field = new UserdefBoolean(values[i], uDef);
											ValidateAndAddField(field, values[i], ref errorString);
											break;
										case "D":
											UserdefDate uDate = new UserdefDate(values[i], uDef);
											ValidateAndAddField(uDate, values[i], ref errorString);
											break;
										case "E":
											UserdefEnumeration uEnum = 
												new UserdefEnumeration(values[i], uDef);
											ValidateAndAddField(uEnum, values[i], ref errorString);
											break;
										case "L":
											UserdefLocation uLoc =
												new UserdefLocation(values[i], uDef);
											ValidateAndAddField(uLoc, values[i], ref errorString);
											break;
										case "M":
											UserdefMultilineString uMS =
												new UserdefMultilineString(values[i], uDef);
											ValidateAndAddField(uMS, values[i], ref errorString);
											break;
										case "N":
											UserdefNumber uNum =
												new UserdefNumber(values[i], uDef);
											ValidateAndAddField(uNum, values[i], ref errorString);
											break;
										case "S":
											UserdefString uStr =
												new UserdefString(values[i], uDef);
											ValidateAndAddField(uStr, values[i], ref errorString);
											break;
										case "T":
											UserdefTime uTime =
												new UserdefTime(values[i], uDef);
											ValidateAndAddField(uTime, values[i], ref errorString);
											break;
										default:
											errorString += string.Format("'{0}' has unsupported data type." +
											                             " Field deleted." +
											                             Environment.NewLine, flds[i]);
											break;
									}
									userDefFound = true;
									break;
								}
							}
							if(!userDefFound)
							{
								errorString += string.Format("'{0}' field not valid field type and" +
								                             " not a user defined type. Field deleted." +
								                             Environment.NewLine, flds[i]);
							}
						}
						break;
				}
			}
		}
		
		/// <summary>
		/// Validate the AdifField and add to list of fields if valid
		/// </summary>
		/// <param name="field">AdifField object to validate</param>
		/// <param name="value">field value</param>
		/// <param name="errorString">string containing error and modified messages</param>
		private void ValidateAndAddField(AdifField field, string value, ref string errorString)
		{
			string err = string.Empty;
			string mod = string.Empty;
			bool valid = field.Validate(out err, out mod);
			if(mod != null)
			{
				errorString += string.Format("{0}:{1} - {2} - Value modified.",
				                             field.Name, field.Value, mod);
			}
			if(!valid)
			{
				errorString += string.Format("{0}:{1} - {2} - Field deleted.",
				                             field.Name, field.Value, err);
			}
			else
			{
				fields.Add(field);
			}
		}
		
		/// <summary>
		/// Validate the QSO. This ensures that the QSO has at least the minimum fields:
		/// call, mode, qso_date, time_on, and either band or freq.
		/// </summary>
		/// <param name="err">Error message if QSO is not valid, null otherwise.</param>
		/// <returns>true if QSO is valid, false otherwise</returns>
		public bool Validate(ref string err)
		{
			err = null;
			string call = this["call"];
			if(call == null || call == string.Empty)
			{
				err = "Invalid QSO: Call not specified.";
				return false;
			}
			string mode = this["mode"];
			if(mode == null || mode == string.Empty)
			{
				err = "Invalid QSO: Mode not specified.";
				return false;
			}
			string freq = this["freq"];
			string band = this["band"];
			bool forb = (freq != null && freq != string.Empty) ||
				(band != null && band != string.Empty);
			if(!forb)
			{
				err = "Invalid QSO: Neither a band or frequency specified.";
				return false;
			}
			string date = this["qso_date"];
			if(date == null || date == string.Empty)
			{
				err = "Invalid QSO: Qso_Date not specified.";
				return false;
			}
			string time = this["time_on"];
			if(time == null || time == string.Empty)
			{
				err = "Invalid QSO: Time_On not specified.";
				return false;
			}
			return true;
		}
		
        /// <summary>
        /// retrieve value associated with key
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>value associated with the key, or null if no field with that key in this Qso</returns>
        /// <exception>ArgumentNullException if the key is null
        /// ArgumentException if key is empty
        /// KeyNotFoundException if no field with that key in this Qso
        public string this[string key]
        {
            get
            {
                if (key == null)
                    throw new ArgumentNullException();
                if (key == string.Empty)
                    throw new ArgumentException("Empty key");
                foreach(AdifField field in fields)
                {
                	if(field.Name.ToLower().Equals(key.ToLower()))
                	{
                		return field.Value;
                	}
                }
                return null;
            }
            set
            {
                if (key == null)
                {
                    throw new ArgumentNullException();
                }
                if (key == string.Empty)
                {
                    ArgumentException ex = new ArgumentException();
                    throw ex;
                }
                foreach(AdifField field in fields)
                {
                	if(field.Name.ToLower().Equals(key.ToLower()))
                	{
                		field.Value = value;
                		string error = string.Empty;
                		string modStr = string.Empty;
                		bool valid = field.Validate(out error, out modStr);
                		if(!valid)
                		{
                			throw new ArgumentException("Invalid value specified for field '" +
                			                            key + "'.");
                		}
                		return;
                	}
                }
                
                // not an existing field, so
                // use new Qso2 object to validate that key is a proper field type
                // and value is valid
                string err = string.Empty;
                string fld = string.Format("<{0}:{1}>{2}", key, value.Length, value);
                Qso2 q = new Qso2(fld, adifEnums, ref err, qsos2);
                if(err != null)
                {
                	string error = "Programming Exception while attempting to add a new field:" +
	                               Environment.NewLine +
	                               err;
                	throw new ArgumentException(error);
                }
                fields.Add(q.Fields[0]);
            }
        }
        
        /// <summary>
        /// retrieve value associated with key, or defaultValue if no field for key
        /// </summary>
        public string this[string key, string defaultValue]
        {
            get
            {
            	string value = this[key];
            	if(value == null)
            	{
            		value = defaultValue;
            	}
                return value;
            }
        }

        /// <summary>
        /// Create an Adif Qso record from the fields in this object
        /// </summary>
        /// <returns>string containing the record</returns>
        public string ToAdifString()
        {
        	string adif = string.Empty;
			foreach(AdifField field in fields)
			{
				adif += field.ToAdifString();
			}
			if(adif != string.Empty)
			{
				adif += "<eor>";
			}
			else
			{
				adif = null;
			}
			return adif;
        }
        
        /// <summary>
        /// compares a Qso2 object to this one for equality
        /// </summary>
        /// <param name="obj">Qso2 object to compare to this one</param>
        /// <returns>true if equal, false otherwise</returns>
        public override bool Equals(object obj)
		{
        	if(obj == null || !(obj is Qso2))
        	{
        		return false;
        	}
			Qso2 other = obj as Qso2;
			if(this.Fields.Count != other.Fields.Count)
			{
				return false;
			}
			foreach(AdifField field1 in this.Fields)
			{
				bool equal = false;
				foreach(AdifField field2 in other.Fields)
				{
					if(field1.Name.Equals(field2.Name))
					{
						if(field1.Equals(field2))
						{
							equal = true;
							break;
						}
						else
						{
							return false;
						}
					}
				}
				if(!equal)
				{
					return false;
				}
			}
			return true;
		}

        /// <summary>
        /// Generate hash code for this object
        /// </summary>
        /// <returns>hash code for this object</returns>
		public override int GetHashCode()
		{
			return this.ToAdifString().GetHashCode();
		}

	}
}
