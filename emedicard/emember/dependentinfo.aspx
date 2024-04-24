<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="dependentinfo.aspx.vb" Inherits="emedicard.dependentinfo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
<div id="dependent_dialog" title="Dependent's Information">
	<form name="dependent" id="dependent" method="POST" action="">
		<div class="innerCont4">PERSONAL INFORMATION (DEPENDENT)</div>
		<table width="100%" border="0">
			<tr>
				<td>
					<label for="d_lastname">Last Name:<span class="h1">*</span></label>
						<input size="15" name="d_lastname" id="d_lastname" type="text" class="required" />
				</td>
				<td>
					<label for="d_firstname">First Name:<span class="h1">*</span></label>
						<input size="15" name="d_firstname" id="d_firstname" type="text"class="required" />
				</td>
				<td>
					<label for="d_middlename">Middle Name:<span class="h1">*</span></label>
						<input size="15" name="d_middlename" id="d_middlename" type="text"class="required" />
				</td>
				<td>
					<label for="d_sex">Sex:<span class="h1">*</span></label>
						<select onChange="getGender(this.value)" name="d_sex" id="d_sex" class="required" >
							<option value=""></option>
							<option value="MALE">MALE</option>
							<option value="FEMALE">FEMALE</option>
						</select>
				</td>
			</tr>
			<tr>
				<td colspan="2">
					<label for="d_address">Address:<span class="h1">*</span></label>
						<input size="55" name="d_address" id="d_address" type="text" class="required" />
				</td>
				<td>							
					<label for="d_city">City:<span class="h1">*</span></label>
						<input name="d_city" id="d_city" type="text" class="required" />
				</td>
				<td>
					<label for="d_province">Province:<span class="h1">*</span></label>
						<input name="d_province" id="d_province" type="text" class="required" />
				</td>
			</tr>
			<tr>
				<td>
					<label for="d_telephone">Tel. No.:<span class="h1">*</span></label>
						<input name="d_telephone" id="d_telephone" type="text" class="required" />
				</td>
				<td>
					<label for="d_mobile">Mobile No.:<span class="h1">*</span></label>
						<input name="d_mobile" id="d_mobile" type="text" class="required" />
				</td>
				<td>
					<label for="d_relationship">Relationship:<span class="h1">*</span></label>
						<input name="d_relationship" id="d_relationship" type="text" class="required" />
				</td>
			</tr>
			<tr>
				<td>
					<label for="d_birthday">Birthday:<span class="h1">*</span></label>
						<input readonly="readonly" name="d_birthday" id="d_birthday" type="text" class="required" />
				</td>
				<td>
					<label for="d_civilstatus">Civil Status:<span class="h1">*</span></label>
						<select name="d_civilstatus" id="d_civilstatus" class="required" >
							<option value=""></option>
							<option value="SINGLE">SINGLE</option>
							<option value="MARRIED">MARRIED</option>
							<option value="WIDOWER">WIDOW</option>
							<option value="SEPARATED">SEPARATED</option>
						</select>
				</td>
				<td>
					<label for="hght">Height(ft):<span class="h1">*</span></label>
						<input size="5" name="d_height" id="d_height" type="text" class="required" />
				</td>
				<td>
					<label for="weight">Weight(lbs):<span class="h1">*</span></label>
						<input size="5" name="d_weight" id="d_weight" type="text" class="required" />
				</td>
			</tr>
			<tr>
				<td colspan="2">
					<label for="d_occupation">Occupation:<span class="h1">*</span></label>
						<input name="d_occupation" id="d_occupation" type="text" class="required" />
				</td>
				<td>
					<label for="d_gsis">GSIS:</label>
						<input name="d_gsis" id="d_gsis" type="text" />
				</td>
				<td>
					<label for="d_sss">SSS:</label>
						<input name="d_sss" id="d_sss" type="text" />
				</td>
			</tr>
		</table>
		<div class="innerCont4">MEDICAL INFORMATION (DEPENDENT)</div>
			<table width="100%" border="0" cellpadding="5">
				<tr class="row1">
					<td></td>
					<td><strong>1. Have you ever treated for or ever had any known indication of:</strong></td>
				</tr>
				<tr class="row2">
					<td width="110px">
						<input onClick="toggle('d_q1a', 'y')" name="d_q1a" id="d_q1ay" value="yes" type="radio" validate="required:true" /><label for="d_q1ay">Yes</label>
						<input onClick="toggle('d_q1a', 'n')" name="d_q1a" id="d_q1an" value="no" type="radio" /><label for="d_q1an">No</label>
					</td>
					<td>
						a. Disorder of eyes, ears, nose, or throat?
						<div id="d_q1aDiv" style="display: none;">
							If <strong>Yes</strong> give details:
							<textarea id="d_q1aTxt" name="d_q1aTxt" class="yesText"></textarea>
						</div>
					</td>
				</tr>
				<tr class="row1">
					<td>
						<input onClick="toggle('d_q1b', 'y')" name="d_q1b" id="d_q1by" value="yes" type="radio" validate="required:true" /><label for="d_q1by">Yes</label>
						<input onClick="toggle('d_q1b', 'n')" name="d_q1b" id="d_q1bn" value="no" type="radio" /><label for="d_q1bn">No</label>
					</td>
					<td>
						b. Dizziness, fainting, convulsions, headache, speech defect, paralysis or stroke, mental or nervous disorder? 
						<div id="d_q1bDiv" style="display: none;">
							If <strong>Yes</strong> give details:
							<textarea id="d_q1bTxt" name="d_q1bTxt" class="yesText">
							</textarea>
						</div>
					</td>
				</tr>
				<tr class="row2">
					<td>
						<input onClick="toggle('d_q1c', 'y')" name="d_q1c" id="d_q1cy" value="yes" type="radio" validate="required:true" /><label for="d_q1cy">Yes</label>
						<input onClick="toggle('d_q1c', 'n')" name="d_q1c" id="d_q1cn" value="no" type="radio" /><label for="d_q1cn">No</label>
					</td>
					<td>
						c. Shortness of breath, persistent hoarseness or cough, blood-spitting bronchitis, pleurisy, asthma, emphysema, tuberculosis or chronic respiratory disorder? 
						<div id="d_q1cDiv" style="display: none;">
							If <strong>Yes</strong> give details:
							<textarea id="d_q1cTxt" name="d_q1cTxt" class="yesText">
							</textarea>
						</div>
					</td>
				</tr>
				<tr class="row1">
					<td>
						<input onClick="toggle('d_q1d', 'y')" name="d_q1d" id="d_q1dy" value="yes" type="radio" validate="required:true" /><label for="d_q1dy">Yes</label>
						<input onClick="toggle('d_q1d', 'n')" name="d_q1d" id="d_q1dn" value="no" type="radio" /><label for="d_q1dn">No</label>
					</td>
					<td>
						d. Chest pain, palpitation, high blood pressure, rheumatic fever, heart murmur, heart attack or any other disorder of the heart or blood vessels?
						<div id="d_q1dDiv" style="display: none;">
							If <strong>Yes</strong> give details:
							<textarea id="d_q1dTxt" name="d_q1dTxt" class="yesText">
							</textarea>
						</div>
					</td>
				</tr>
				<tr class="row2">
					<td>
						<input onClick="toggle('d_q1e', 'y')" name="d_q1e" id="d_q1ey" value="yes" type="radio" validate="required:true" /><label for="d_q1ey">Yes</label>
						<input onClick="toggle('d_q1e', 'n')" name="d_q1e" id="d_q1en" value="no" type="radio" /><label for="d_q1en">No</label>
					</td>
					<td>
						e. Jaundice intestinal bleeding, ulcer, hernia, appendicitis, diverticulitis colitis, hemorrhoids, recurrent indigestion, or other disorder of the stomach intestine, liver or gallbladder?
						<div id="d_q1eDiv" style="display: none;">
							If <strong>Yes</strong> give details:
							<textarea id="d_q1eTxt" name="d_q1eTxt" class="yesText">
							</textarea>
						</div>
					</td>
				</tr>
				<tr class="row1">
					<td>
						<input onClick="toggle('d_q1f', 'y')" name="d_q1f" id="d_q1fy" value="yes" type="radio" validate="required:true" /><label for="d_q1fy">Yes</label>
						<input onClick="toggle('d_q1f', 'n')" name="d_q1f" id="d_q1fn" value="no" type="radio" /><label for="d_q1fn">No</label>
					</td>
					<td>
						f. Sugar, albumin, blood or pus in urine, venereal disease, stone or other disorder of kidney, bladder, prostate or reproductive organs?
						<div id="d_q1fDiv" style="display: none;">
							If <strong>Yes</strong> give details:
							<textarea id="d_q1fTxt" name="d_q1fTxt" class="yesText">
							</textarea>
						</div>
					</td>
				</tr>
				<tr class="row2">
					<td>
						<input onClick="toggle('d_q1g', 'y')" name="d_q1g" id="d_q1gy" value="yes" type="radio" validate="required:true" /><label for="d_q1gy">Yes</label>
						<input onClick="toggle('d_q1g', 'n')" name="d_q1g" id="d_q1gn" value="no" type="radio" /><label for="d_q1gn">No</label>
					</td>
					<td>
						g. Diabetes thyroid or other endocrine disorder?
						<div id="d_q1gDiv" style="display: none;">
							If <strong>Yes</strong> give details:
							<textarea id="d_q1gTxt" name="d_q1gTxt" class="yesText">
							</textarea>
						</div>
					</td>
				</tr>
				<tr class="row1">
					<td>
						<input onClick="toggle('d_q1h', 'y')" name="d_q1h" id="d_q1hy" value="yes" type="radio" validate="required:true" /><label for="d_q1hy">Yes</label>
						<input onClick="toggle('d_q1h', 'n')" name="d_q1h" id="d_q1hn" value="no" type="radio" /><label for="d_q1hn">No</label>
					</td>
					<td>
						h. Neuritis, sciatica, rheumatism, arthritis, gout, or disorder of the muscles or bones, such as spine, back or joints?
						<div id="d_q1hDiv" style="display: none;">
							If <strong>Yes</strong> give details:
							<textarea id="d_q1hTxt" name="d_q1hTxt" class="yesText">
							</textarea>
						</div>
					</td>
				</tr>
				<tr class="row2">
					<td>
						<input onClick="toggle('d_q1i', 'y')" name="d_q1i" id="d_q1iy" value="yes" type="radio" validate="required:true" /><label for="d_q1iy">Yes</label>
						<input onClick="toggle('d_q1i', 'n')" name="d_q1i" id="d_q1in" value="no" type="radio" /><label for="d_q1in">No</label>
					</td>
					<td>
						i. Deformity, lameness or amputation?
						<div id="d_q1iDiv" style="display: none;">
							If <strong>Yes</strong> give details:
							<textarea id="d_q1iTxt" name="d_q1iTxt" class="yesText">
							</textarea>
						</div>
					</td>
				</tr>
				<tr class="row1">
					<td>
						<input onClick="toggle('d_q1j', 'y')" name="d_q1j" id="d_q1jy" value="yes" type="radio" validate="required:true" /><label for="d_q1jy">Yes</label>
						<input onClick="toggle('d_q1j', 'n')" name="d_q1j" id="d_q1jn" value="no" type="radio" /><label for="d_q1jn">No</label>
					</td>
					<td>
						j. Disorder of skin, lymph glands, cysts, tumor or cancer?
						<div id="d_q1jDiv" style="display: none;">
							If <strong>Yes</strong> give details:
							<textarea id="d_q1jTxt" name="d_q1jTxt" class="yesText">
							</textarea>
						</div>
					</td>
				</tr>
				<tr class="row2">
					<td>
						<input onClick="toggle('d_q1k', 'y')" name="d_q1k" id="d_q1ky" value="yes" type="radio" validate="required:true" /><label for="d_q1ky">Yes</label>
						<input onClick="toggle('d_q1k', 'n')" name="d_q1k" id="d_q1kn" value="no" type="radio" /><label for="d_q1kn">No</label>
					</td>
					<td>
						k. Allergies, anemia or other disorder of the blood?
						<div id="d_q1kDiv" style="display: none;">
							If <strong>Yes</strong> give details:
							<textarea id="d_q1kTxt" name="d_q1kTxt" class="yesText">
							</textarea>
						</div>
					</td>
				</tr>
				<tr class="row1">
					<td>
						<input onClick="toggle('d_q1l', 'y')" name="d_q1l" id="d_q1ly" value="yes" type="radio" validate="required:true" /><label for="d_q1ly">Yes</label>
						<input onClick="toggle('d_q1l', 'n')" name="d_q1l" id="d_q1ln" value="no" type="radio" /><label for="d_q1ln">No</label>
					</td>
					<td>
						l. Excessive use of alcohol, tobacco or any habit-forming drug?
						<div id="d_q1lDiv" style="display: none;">
							If <strong>Yes</strong> give details:
							<textarea id="d_q1lTxt" name="d_q1lTxt" class="yesText">
							</textarea>
						</div>
					</td>
				</tr>
				<tr class="row2">
					<td>
						<input name="d_q2" id="d_q2y" value="yes" type="radio"><label for="d_q2y">Yes</label>
						<input name="d_q2" id="d_q2n" value="no" type="radio"><label for="d_q2n">No</label>
					</td>
					<td><strong>2. Are you now under observation or taking treatment?</strong></td>
				</tr>
				<tr class="row1">
					<td>
						<input onClick="toggle('d_q3', 'y')" name="d_q3" id="d_q3y" value="yes" type="radio" validate="required:true" /><label for="d_q3y">Yes</label>
						<input onClick="toggle('d_q3', 'n')" name="d_q3" id="d_q3n" value="no" type="radio" /><label for="d_q3n">No</label>
					</td>
					<td>
						<strong>3. Do you smoke cigarette?</strong>
						<div id="d_q3Div" style="display: none;">
							If so, how many sticks a day?
							<textarea id="d_q3Txt" name="d_q3Txt" class="yesText">
							</textarea>
						</div>
					</td>
				</tr>
				<tr class="row2">
					<td></td>
					<td><strong>4. Other than above, have you:</strong></td>
				</tr>
				<tr class="row1">
					<td>
						<input onClick="toggle('d_q4a', 'y')" name="d_q4a" id="d_q4ay" value="yes" type="radio" validate="required:true" /><label for="d_q4ay">Yes</label>
						<input onClick="toggle('d_q4a', 'n')" name="d_q4a" id="d_q4an" value="no" type="radio" /><label for="d_q4an">No</label>
					</td>
					<td>
						a. Had any physical disorder or any known indication thereof?
						<div id="d_q4aDiv" style="display: none;">
							If <strong>Yes</strong> give details:
							<textarea id="d_q4aTxt" name="d_q4aTxt" class="yesText">
							</textarea>
						</div>
					</td>
				</tr>
				<tr class="row2">
					<td>
						<input onClick="toggle('d_q4b', 'y')" name="d_q4b" id="d_q4by" value="yes" type="radio" validate="required:true" /><label for="d_q4by">Yes</label>
						<input onClick="toggle('d_q4b', 'n')" name="d_q4b" id="d_q4bn" value="no" type="radio" /><label for="d_q4bn">No</label>
					</td>
					<td>
						b. Had a medical examination, consultations, illness, injury, surgery?
						<div id="d_q4bDiv" style="display: none;">
							If <strong>Yes</strong> give details:
							<textarea id="d_q4bTxt" name="d_q4bTxt" class="yesText">
							</textarea>
						</div>
					</td>
				</tr>
				<tr class="row1">
					<td>
						<input onClick="toggle('d_q4c', 'y')" name="d_q4c" id="d_q4cy" value="yes" type="radio" validate="required:true" /><label for="d_q4cy">Yes</label>
						<input onClick="toggle('d_q4c', 'n')" name="d_q4c" id="d_q4cn" value="no" type="radio" /><label for="d_q4cn">No</label>
					</td>
					<td>
						c. Been a patient in a hospital, clinic, sanitarium, or other medical facility?
						<div id="d_q4cDiv" style="display: none;">
							If <strong>Yes</strong> give details:
							<textarea id="d_q4cTxt" name="d_q4cTxt" class="yesText">
							</textarea>
						</div>
					</td>
				</tr>
				<tr class="row2">
					<td>
						<input onClick="toggle('d_q4d', 'y')" name="d_q4d" id="d_q4dy" value="yes" type="radio" validate="required:true" /><label for="d_q4dy">Yes</label>
						<input onClick="toggle('d_q4d', 'n')" name="d_q4d" id="d_q4dn" value="no" type="radio" /><label for="d_q4dn">No</label>
					</td>
					<td>
						d. Had electrocardiogram, x-ray, other diagnostic test?
						<div id="d_q4dDiv" style="display: none;">
							If <strong>Yes</strong> give details:
							<textarea id="d_q4dTxt" name="d_q4dTxt" class="yesText">
							</textarea>
						</div>
					</td>
				</tr>
				<tr class="row1">
					<td>
						<input onClick="toggle('d_q4e', 'y')" name="d_q4e" id="d_q4ey" value="yes" type="radio" validate="required:true" /><label for="d_q4ey">Yes</label>
						<input onClick="toggle('d_q4e', 'n')" name="d_q4e" id="d_q4en" value="no" type="radio" /><label for="d_q4en">No</label>
					</td>
					<td>
						e. Been advised to have a diagnostic test, hospitalization, or surgery which was not completed?
						<div id="d_q4eDiv" style="display: none;">
							If <strong>Yes</strong> give details:
							<textarea id="d_q4eTxt" name="d_q4eTxt" class="yesText">
							</textarea>
						</div>
					</td>
				</tr>
				<tr class="row2">
					<td>
						<input name="d_q5" id="d_q5y" value="yes" type="radio" validate="required:true" /><label for="d_q5y">Yes</label>
						<input name="d_q5" id="d_q5n" value="no" type="radio" /><label for="d_q5n">No</label>
					</td>
					<td><strong>5. Have you ever had military service deferment, rejection or discharge because of physical or mental condition?</strong></td>
				</tr>
				<tr class="row1">
					<td>
						<input name="d_q6" id="d_q6y" value="yes" type="radio" validate="required:true" /><label for="d_q6y">Yes</label>
						<input name="d_q6" id="d_q6n" value="no" type="radio" /><label for="d_q6n">No</label>
					</td>
					<td><strong>6. Have you ever applied for or receive a pension, payment, or benefit due to injury, sickness or disability?</strong></td>
				</tr>
				<tr class="row2">
					<td>
						<input name="d_q7" id="d_q7y" value="yes" type="radio" validate="required:true" /><label for="d_q7y">Yes</label>
						<input name="d_q7" id="d_q7n" value="no" type="radio" /><label for="d_q7n">No</label>
					</td>
					<td><strong>7. Have you a parent, brother, sister who died of or had high blood pressure, tubercolosis, diabetes, cancer, heart or kidney disease, or mental illness?</strong></td>
				</tr>
				<tr class="row1">
					<td></td>
					<td><strong>8. FOR FEMALES ONLY:</strong></td>
				</tr>
				<tr class="row2">
					<td>
						<input onClick="toggle('d_q8a', 'y')" name="d_q8a" id="d_q8ay" value="yes" type="radio" validate="required:true" /><label for="d_q8ay">Yes</label>
						<input onClick="toggle('d_q8a', 'n')" name="d_q8a" id="d_q8an" value="no" type="radio" /><label for="d_q8an">No</label>
					</td>
					<td>
						a. Have you ever had any abnormal menstruation, pregnancy, childbirth or disorder of the female organs or breast?
						<div id="d_q8aDiv" style="display: none;">
							If <strong>Yes</strong> give details:
							<textarea id="d_q8aTxt" name="d_q8aTxt" class="yesText">
							</textarea>
						</div>
					</td>
				</tr>
				<tr class="row1">
					<td>
						<input onClick="toggle('d_q8b', 'y')" name="d_q8b" id="d_q8by" value="yes" type="radio" validate="required:true" /><label for="d_q8by">Yes</label>
						<input onClick="toggle('d_q8b', 'n')" name="d_q8b" id="d_q8bn" value="no" type="radio" /><label for="d_q8bn">No</label>
					</td>
					<td>
						b. Are you now pregnant?
						<div id="d_q8bDiv" style="display: none;">
							If <strong>Yes</strong>, how many months?
							<textarea id="d_q8bTxt" name="d_q8bTxt" class="yesText">
							</textarea>
						</div>
					</td>
				</tr>
				<tr class="row2">
					<td>
						<input onClick="toggle('d_q8c', 'y')" name="d_q8c" id="d_q8cy" value="yes" type="radio" validate="required:true" /><label for="d_q8cy">Yes</label>
						<input onClick="toggle('d_q8c', 'n')" name="d_q8c" id="d_q8cn" value="no" type="radio" /><label for="d_q8cn">No</label>
					</td>
					<td>
						c. Are you taking contraceptives pills?
						<div id="d_q8cDiv" style="display: none;">
							If <strong>Yes</strong> give details:
							<textarea id="d_q8cTxt" name="d_q8cTxt" class="yesText">
							</textarea>
						</div>
					</td>
				</tr>
				<tr class="row1">
					<td>
						<input onClick="toggle('d_q9', 'y')" name="d_q9" id="d_q9y" value="yes" type="radio" validate="required:true" /><label for="d_q9y">Yes</label>
						<input onClick="toggle('d_q9', 'n')" name="d_q9" id="d_q9n" value="no" type="radio" /><label for="d_q9n">No</label>
					</td>
					<td>
						<strong>
						9. Have you ever been rejected or terminated for medical insurance 
						including MEDICard program, or have been offered insurance at a higher (rated-up) premium?
						</strong>
						<div id="d_q9Div" style="display: none;">
							If <strong>Yes</strong> give details:
							<textarea id="d_q9Txt" name="d_q9Txt" class="yesText">
							</textarea>
						</div>
					</td>
				</tr>
			</table>
	</form>
</div>
</body>
</html>
